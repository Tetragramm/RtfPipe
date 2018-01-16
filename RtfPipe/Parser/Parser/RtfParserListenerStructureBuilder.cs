// -- FILE ------------------------------------------------------------------
// name       : RtfParserListenerStructureBuilder.cs
// project    : RTF Framelet
// created    : Leon Poyyayil - 2008.05.19
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System.Collections;
using RtfPipe.Model;
using System.Collections.Generic;

namespace RtfPipe.Parser
{

	// ------------------------------------------------------------------------
	public sealed class RtfParserListenerStructureBuilder : RtfParserListenerBase
	{

		// ----------------------------------------------------------------------
		public IRtfGroup StructureRoot
		{
			get { return structureRoot; }
		} // StructureRoot

		// ----------------------------------------------------------------------
		protected override void DoParseBegin()
		{
			openGroupStack.Clear();
			curGroup = null;
			structureRoot = null;
		} // DoParseBegin

		// ----------------------------------------------------------------------
		protected override void DoGroupBegin()
		{
			RtfGroup newGroup = new RtfGroup();
			if ( curGroup != null )
			{
				openGroupStack.Push( curGroup );
				curGroup.Contents.Add( newGroup );
			}
			curGroup = newGroup;
		} // DoGroupBegin

		// ----------------------------------------------------------------------
		protected override void DoTagFound( IRtfTag tag )
		{
			if ( curGroup == null )
			{
				throw new RtfStructureException( Strings.MissingGroupForNewTag );
			}
			curGroup.Contents.Add( tag );
		} // DoTagFound

		// ----------------------------------------------------------------------
		protected override void DoTextFound( IRtfText text )
		{
			if ( curGroup == null )
			{
				throw new RtfStructureException( Strings.MissingGroupForNewText );
			}
			curGroup.Contents.Add( text );
		} // DoTextFound

		// ----------------------------------------------------------------------
		protected override void DoGroupEnd()
		{
			if ( openGroupStack.Count > 0 )
			{
				curGroup = openGroupStack.Pop();
			}
			else
			{
				if ( structureRoot != null )
				{
					throw new RtfStructureException( Strings.MultipleRootLevelGroups );
				}
				structureRoot = curGroup;
				curGroup = null;
			}
		} // DoGroupEnd

		// ----------------------------------------------------------------------
		protected override void DoParseEnd()
		{
			if ( openGroupStack.Count > 0 )
			{
				throw new RtfBraceNestingException( Strings.UnclosedGroups );
			}
		} // DoParseEnd

		// ----------------------------------------------------------------------
		// members
		private readonly Stack<RtfGroup> openGroupStack = new Stack<RtfGroup>();
		private RtfGroup curGroup;
		private RtfGroup structureRoot;

	} // class RtfParserListenerStructureBuilder

} // namespace RtfPipe.Parser
// -- EOF -------------------------------------------------------------------