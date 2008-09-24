///////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation. All rights reserved.
///////////////////////////////////////////////////////////////////////////////
//
// Type Library Importer utility
//
// This program imports all the types in the type library into a interop assembly
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace tlbimp2
{
    /// <summary>
    /// Wrapper for TYPEATTR
    /// It used to implement IDisposable because we hope the resource/memory could be released ASAP
    /// However we do need the TypeDesc/ParamDesc/... types that are holding a IntPtr to function correctly,
    /// so change this to rely on GC instead, assuming that TypeLib API doesn't attach any value resources
    /// to FUNCDESC/VARDESC/TYPEATTR/TYPELIBATTR
    /// Note: We still keep IDispoable for now in order to minimize source code change
    /// </summary>
    class TypeAttr : IDisposable
    {
        public TypeAttr(ITypeInfo typeinfo)
        {
            m_typeinfo = typeinfo;
            typeinfo.GetTypeAttr(out m_ipTypeAttr);
            m_typeattr = (TYPEATTR)Marshal.PtrToStructure(m_ipTypeAttr, typeof(TYPEATTR));
        }

        ~TypeAttr()
        {
            if (m_ipTypeAttr != IntPtr.Zero)
                m_typeinfo.ReleaseTypeAttr(m_ipTypeAttr);

            m_typeinfo = null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            // Do nothing. 
            // We keep this to minimize source code change for now
        }

        #endregion

        public Guid Guid { get { return m_typeattr.guid; } }
        public int cbSizeInstance { get { return m_typeattr.cbSizeInstance; } }
        public TYPEKIND typekind { get { return m_typeattr.typekind; } }
        public int cFuncs { get { return m_typeattr.cFuncs; } }
        public int cVars { get { return m_typeattr.cVars; } }
        public int cImplTypes { get { return m_typeattr.cImplTypes; } }
        public int cbSizeVft { get { return m_typeattr.cbSizeVft; } }
        public int cbAlignment { get { return m_typeattr.cbAlignment; } }
        public TYPEFLAGS wTypeFlags { get { return m_typeattr.wTypeFlags; } }
        public int wMajorVerNum { get { return m_typeattr.wMajorVerNum; } }
        public int wMinorVerNum { get { return m_typeattr.wMinorVerNum; } }
        public TypeDesc tdescAlias { get { return new TypeDesc(this, m_typeattr.tdescAlias.desc.lptdesc, m_typeattr.tdescAlias.vt); } }
        public IdlDesc ideldescType { get { return new IdlDesc(m_typeattr.idldescType.wIDLFlags); } }

        // Expand out TYPEKIND
        public bool IsEnum { get { return typekind == TYPEKIND.TKIND_ENUM; } }
        public bool IsRecord { get { return typekind == TYPEKIND.TKIND_RECORD; } }
        public bool IsModule { get { return typekind == TYPEKIND.TKIND_MODULE; } }
        public bool IsInterface { get { return typekind == TYPEKIND.TKIND_INTERFACE; } }
        public bool IsDispatch { get { return typekind == TYPEKIND.TKIND_DISPATCH; } }
        public bool IsCoClass { get { return typekind == TYPEKIND.TKIND_COCLASS; } }
        public bool IsAlias { get { return typekind == TYPEKIND.TKIND_ALIAS; } }
        public bool IsUnion { get { return typekind == TYPEKIND.TKIND_UNION; } }

        // Expand out TYPEFLAGS
        public bool IsAppObject { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FAPPOBJECT) != 0; } }
        public bool IsCanCreate { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FCANCREATE) != 0; } }
        public bool IsLicensed { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FLICENSED) != 0; } }
        public bool IsPreDeclId { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FPREDECLID) != 0; } }
        public bool IsHidden { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FHIDDEN) != 0; } }
        public bool IsControl { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FCONTROL) != 0; } }
        public bool IsDual { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FDUAL) != 0; } }
        public bool IsNonExtensible { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FNONEXTENSIBLE) != 0; } }
        public bool IsOleAutomation { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FOLEAUTOMATION) != 0; } }
        public bool IsRestricted { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FRESTRICTED) != 0; } }
        public bool IsAggregatable { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FAGGREGATABLE) != 0; } }
        public bool IsReplaceable { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FREPLACEABLE) != 0; } }
        public bool IsDispatchable { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FDISPATCHABLE) != 0; } }
        public bool IsReverseBind { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FREVERSEBIND) != 0; } }
        public bool IsProxy { get { return (wTypeFlags & TYPEFLAGS.TYPEFLAG_FPROXY) != 0; } }

        private TYPEATTR m_typeattr;
        private IntPtr m_ipTypeAttr;
        private ITypeInfo m_typeinfo;
    }
}
