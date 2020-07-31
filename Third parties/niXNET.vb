
'The attached Code is provided As Is. It has not been tested or validated as a product, 
'for use in a deployed application or system, or for use in hazardous environments. You 
'assume all risks for use of the Code and use of the Code is subject to the Sample Code 
'License Terms which can be found at: http://ni.com/samplecodelicense 



'Functions that are commented were automatically generated and have not been tried
' They were let in the file as a starting point to extend the wrapper.
' Please see the readme for more information about the list of available functions.

Imports System.Runtime.InteropServices
Namespace NationalInstruments.EmbeddedNetworks.Interop

    Public Class niXNET
        Inherits Object
        Implements System.IDisposable
        'Private _handle As System.IntPtr

        Private _disposed As Boolean = True

        Private _handle As System.IntPtr
        Public Property SessionRef() As System.IntPtr
            Get
                Return _handle
            End Get
            Set(ByVal value As System.IntPtr)
                _handle = value
            End Set
        End Property


        Protected Overrides Sub Finalize()
            Try
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

        '**********************************************************************
        '                                    D A T A   T Y P E S
        '        **********************************************************************

        ' The ANSI C99 standard defines simple numeric types of a specific size,
        '           such as int32_t for a signed 32-bit integer.
        '           Many C/C++ compilers are not ANSI C99 by default, such as Microsoft Visual C/C++.
        '           Therefore, NI-XNET does not require use of ANSI C99.
        '           Since NI-XNET does not attempt to override ANSI C99 types (as defined in stdint.h),
        '           it uses legacy National Instruments numeric types such as i32. If desired, you can use
        '           ANSI C99 numeric types instead of the analogous NI-XNET numeric type
        '           (i.e. int32_t instead of i32). 


        'typedef void*              nxVoidPtr;
        'typedef uint*               nxU32Ptr;

        ' Session Reference (handle).
        'typedef uint nxSessionRef_t;

        ' Database Reference (handle).
        'typedef u32 nxDatabaseRef_t;

        'typedef i32 nxStatus_t;       // Return value

        ' Absolute timestamp.
        'typedef u64 nxTimestamp_t;

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure niFlexRayStats
            Public NumSyntaxErrorChA As UInteger
            Public NumSyntaxErrorChB As UInteger
            Public NumContentErrorChA As UInteger
            Public NumContentErrorChB As UInteger
            Public NumSlotBoundaryViolationChA As UInteger
            Public NumSlotBoundaryViolationChB As UInteger
        End Structure


        'Dispose automatically clears the session.
        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Dispose(True)
            System.GC.SuppressFinalize(Me)
        End Sub

        Private Sub Dispose(ByVal disposing As Boolean)
            If (Me._disposed = False) Then
                PInvoke.nx_clear(Me._handle)
                Me._handle = System.IntPtr.Zero
            End If
            Me._disposed = True
        End Sub
        ' 

        '**********************************************************************
        '               F U N C T I O N   P R O T O T Y P E S  :  S E S S I O N
        '        **********************************************************************

        'typedef u32 nxSessionRef_t;

        ' Database Reference (handle).
        'typedef u32 nxDatabaseRef_t;

        'typedef i32 nxStatus_t;       // Return value

        ' Absolute timestamp.
        'typedef u64 nxTimestamp_t;

        'nxStatus_t _NXFUNC nxCreateSession (
        '                           const char * DatabaseName,
        '                           const char * ClusterName,
        '                           const char * List,
        '                           const char * Interface,
        '                           u32 Mode,
        '                           nxSessionRef_t * SessionRef); /* 

        Public Sub New(ByVal databaseName As String, ByVal clusterName As String, ByVal list As String, ByVal xnetInterface As String, ByVal mode As UInteger)
            Dim pInvokeResult As Integer = PInvoke.CreateSession(databaseName, clusterName, list, xnetInterface, mode, Me._handle)
            PInvoke.TestForError(System.IntPtr.Zero, pInvokeResult)
            Me._disposed = False
        End Sub

        Public Sub New(ByVal numberOfDatabaseRef As UInteger, ByVal arrayOfDatabaseRef As UInteger(), ByVal xnetInterface As String, ByVal mode As UInteger)
            Dim pInvokeResult As Integer = PInvoke.createSessionByRef(numberOfDatabaseRef, arrayOfDatabaseRef, xnetInterface, mode, Me._handle)
            PInvoke.TestForError(System.IntPtr.Zero, pInvokeResult)
            Me._disposed = False
        End Sub

        'nxStatus_t _NXFUNC nxGetProperty (
        '                                   nxSessionRef_t SessionRef,
        '                                   u32 PropertyID,
        '                                   u32 PropertySize,
        '                                   void * PropertyValue); f64 (is double), u32[16], u32[], boolean, double, cstr, nxDatabaseRef_t[] (nxDatabaseRef_t is u32) /* 


        Public Function nx_GetProperty(ByVal propertyID As UInteger, ByVal propertySize As UInteger, ByRef propertyValue As UInteger()) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_getProperty(Me._handle, propertyID, propertySize, propertyValue)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        Public Function nx_GetProperty(ByVal propertyID As UInteger, ByVal propertySize As UInteger, ByRef propertyValue As UInteger) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_getProperty(Me._handle, propertyID, propertySize, propertyValue)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function


        Public Function nx_GetProperty(ByVal propertyID As UInteger, ByVal propertySize As UInteger, ByRef propertyValue As String) As Integer

            Dim propertyPtr As New System.Text.StringBuilder(CInt(propertySize))
            Dim pInvokeResult As Integer = PInvoke.nx_getProperty(Me._handle, propertyID, propertySize, propertyPtr)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            propertyValue = propertyPtr.ToString()
            Return pInvokeResult
        End Function

        'nxStatus_t _NXFUNC nxGetPropertySize (
        '                                   nxSessionRef_t SessionRef,
        '                                   u32 PropertyID,
        '                                   u32 * PropertySize);/* 

        Public Function nx_GetPropertySize(ByVal propertyID As UInteger, ByRef propertySize As UInteger) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_getPropertySize(Me._handle, propertyID, propertySize)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        'nxStatus_t _NXFUNC nxSetProperty (
        '                                   nxSessionRef_t SessionRef,
        '                                   u32 PropertyID,
        '                                   u32 PropertySize,
        '                                   void * PropertyValue);/* 

        Public Function nx_SetProperty(ByVal propertyID As UInteger, ByVal propertySize As UInteger, ByVal propertyValue As IntPtr) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_setProperty(Me._handle, propertyID, propertySize, propertyValue)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        Public Function nx_SetProperty(ByVal propertyID As UInteger, ByVal propertySize As UInteger, ByVal propertyValue As UInteger) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_setProperty(Me._handle, propertyID, propertySize, propertyValue)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function


        'nxStatus_t _NXFUNC nxGetSubProperty (
        '                                   nxSessionRef_t SessionRef,
        '                                   u32 ActiveIndex,
        '                                   u32 PropertyID,
        '                                   u32 PropertySize,
        '        //                           void * PropertyValue);/* 

        'public unsafe int nx_GetSubProperty(uint activeIndex, uint propertyID, uint propertySize, void* propertyValue)
        '{
        '    int pInvokeResult = PInvoke.nx_getSubProperty(this._handle, activeIndex, propertyID, propertySize, propertyValue);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        'nxStatus_t _NXFUNC nxGetSubPropertySize (
        '                                   nxSessionRef_t SessionRef,
        '                                   u32 ActiveIndex,
        '                                   u32 PropertyID,
        '                                   u32 * PropertySize);/* 

        'public int nx_GetSubPropertySize(uint activeIndex, uint propertyID, out uint propertySize)
        '{
        '    int pInvokeResult = PInvoke.nx_getSubPropertySize(this._handle, activeIndex, propertyID, out propertySize);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        'nxStatus_t _NXFUNC nxSetSubProperty (
        '                                   nxSessionRef_t SessionRef,
        '                                   u32 ActiveIndex,
        '                                   u32 PropertyID,
        '                                   u32 PropertySize,
        '                                   void * PropertyValue);/* 

        'public unsafe int nx_SetSubProperty(uint activeIndex, uint propertyID, uint propertySize, void* propertyValue)
        '{
        '    int pInvokeResult = PInvoke.nx_setSubProperty(this._handle, activeIndex, propertyID, propertySize, propertyValue);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        'nxStatus_t _NXFUNC nxReadFrame (
        '                                   nxSessionRef_t SessionRef,
        '                                   void * Buffer,
        '                                   u32 SizeOfBuffer,
        '                                   f64 Timeout,
        '                                   u32 * NumberOfBytesReturned);/* 

        Public Function nx_ReadFrame(ByVal buffer As Byte(), ByVal sizeOfBuffer As UInteger, ByVal timeOut As Double, ByRef numberOfBytesReturned As UInteger) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_readFrame(Me._handle, buffer, sizeOfBuffer, timeOut, numberOfBytesReturned)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        'nxStatus_t _NXFUNC nxReadSignalSinglePoint (
        '                                   nxSessionRef_t SessionRef,
        '                                   f64 * ValueBuffer,
        '                                   u32 SizeOfValueBuffer,
        '                                   nxTimestamp_t * TimestampBuffer,
        '                                   u32 SizeOfTimestampBuffer);/* 

        Public Function nx_ReadSignalSinglePoint(ByVal valueBuffer As Double(), ByVal sizeOfValueBuffer As UInteger, ByVal timestampBuffer As ULong(), ByVal sizeOfTimestampBuffer As UInteger) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_readSignalSinglePoint(Me._handle, valueBuffer, sizeOfValueBuffer, timestampBuffer, sizeOfTimestampBuffer)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        'nxStatus_t _NXFUNC nxReadSignalWaveform (
        '                                   nxSessionRef_t SessionRef,
        '                                   f64 Timeout,
        '                                   nxTimestamp_t * StartTime,
        '                                   f64 * DeltaTime,
        '                                   f64 * ValueBuffer,
        '                                   u32 SizeOfValueBuffer,
        '                                   u32 * NumberOfValuesReturned);/* 

        'public int nx_ReadSignalWaveform(double timeout, out ulong startTime, out double deltaTime, uint sizeOfValueBuffer, out uint numberOfValuesReturned)
        '{
        '    int pInvokeResult = PInvoke.nx_readSignalWaveform(this._handle, timeout, out startTime, out deltaTime, sizeOfValueBuffer, out numberOfValuesReturned);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        'nxStatus_t _NXFUNC nxReadSignalXY (
        '                                   nxSessionRef_t SessionRef,
        '                                   nxTimestamp_t * TimeLimit,
        '                                   f64 * ValueBuffer,
        '                                   u32 SizeOfValueBuffer,
        '                                   nxTimestamp_t * TimestampBuffer,
        '                                   u32 SizeOfTimestampBuffer,
        '                                   u32 * NumPairsBuffer,
        '                                   u32 SizeOfNumPairsBuffer);/* 


        'public int nx_ReadSignalXY(ulong timeLimit, double[,] valueBuffer, uint sizeOfValueBuffer, ulong[,] timestampBuffer,uint sizeOfTimestampBuffer, uint[] numPairsBuffer, uint sizeOfNumPairsBuffer)
        '{
        '    int pInvokeResult = PInvoke.nx_readSignalXY(this._handle, timeLimit, valueBuffer, sizeOfValueBuffer, timestampBuffer, sizeOfTimestampBuffer, numPairsBuffer, sizeOfNumPairsBuffer);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        'nxStatus_t _NXFUNC nxReadState (
        '                                   nxSessionRef_t SessionRef,
        '                                   u32 StateID,
        '                                   u32 StateSize,
        '                                   void * StateValue,
        '                                   nxStatus_t * Fault);/* 

        'public unsafe int nx_ReadState(uint stateID, uint stateSize, void* stateValue, out int fault)
        '{
        '    int pInvokeResult = PInvoke.nx_readState(this._handle, stateID, stateSize, stateValue, out fault);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}
        'Modif Franck suite integration des TimeStamp
        'Public Function nx_ReadState(ByVal stateID As UInteger, ByVal stateSize As UInteger, ByRef stateValue As UInteger, ByRef fault As Integer) As Integer
        '    Dim pInvokeResult As Integer = PInvoke.nx_readState(Me._handle, stateID, stateSize, stateValue, fault)
        '    PInvoke.TestForError(Me._handle, pInvokeResult)
        '    Return pInvokeResult
        'End Function

        Public Function nx_ReadState(ByVal stateID As UInteger, ByVal stateSize As UInteger, ByRef stateValue As System.IntPtr, ByRef fault As Integer) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_readState(Me._handle, stateID, stateSize, stateValue, fault)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function


        'nxStatus_t _NXFUNC nxWriteFrame (
        '                                   nxSessionRef_t SessionRef,
        '                                   void * Buffer,
        '                                   u32 NumberOfBytesForFrames,
        '                                   f64 Timeout);/* 

        Public Function nx_WriteFrame(ByVal buffer As Byte(), ByVal numberOfBytesForFrames As UInteger, ByVal timeOut As Double) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_writeFrame(Me._handle, buffer, numberOfBytesForFrames, timeOut)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        'nxStatus_t _NXFUNC nxWriteSignalSinglePoint (
        '                                   nxSessionRef_t SessionRef,
        '                                   f64 * ValueBuffer,
        '                                   u32 SizeOfValueBuffer);/* 

        Public Function nx_WriteSignalSinglePoint(ByVal valueBuffer As Double(), ByVal sizeOfValueBuffer As UInteger) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_writeSignalSinglePoint(Me._handle, valueBuffer, sizeOfValueBuffer)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        'nxStatus_t _NXFUNC nxWriteState (
        '                                   nxSessionRef_t SessionRef,
        '                                   u32 StateID,
        '                                   u32 StateSize,
        '                                   void * StateValue);/* 

        Public Function nx_WriteState(ByVal stateID As UInteger, ByVal stateSize As UInteger, ByRef stateValue As System.IntPtr) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_writeState(Me._handle, stateID, stateSize, stateValue)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        'nxStatus_t _NXFUNC nxWriteSignalWaveform (
        '                                   nxSessionRef_t SessionRef,
        '                                   f64 Timeout,
        '                                   f64 * ValueBuffer,
        '                                   u32 SizeOfValueBuffer);/* 

        Public Function nx_WriteSignalWaveform(ByVal timeOut As Double, ByVal valueBuffer As Double(,), ByVal sizeOfValueBuffer As UInteger) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_writeSignalWaveform(Me._handle, timeOut, valueBuffer, sizeOfValueBuffer)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        'nxStatus_t _NXFUNC nxWriteSignalXY (
        '                                   nxSessionRef_t SessionRef,
        '                                   f64 Timeout,
        '                                   f64 * ValueBuffer,
        '                                   u32 SizeOfValueBuffer,
        '                                   nxTimestamp_t * TimestampBuffer,
        '                                   u32 SizeOfTimestampBuffer,
        '                                   u32 * NumPairsBuffer,
        '                                   u32 SizeOfNumPairsBuffer);/* 

        Public Function nx_WriteSignalXY(ByVal timeOut As Double, ByVal valueBuffer As Double(,), ByVal sizeOfValueBuffer As UInteger, ByVal timestampBuffer As ULong(,), ByVal sizeOfTimestampBuffer As UInteger, ByVal numPairsBuffer As UInteger(), _
            ByVal sizeOfNumPairsBuffer As UInteger) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_writeSignalXY(Me._handle, timeOut, valueBuffer, sizeOfValueBuffer, timestampBuffer, sizeOfTimestampBuffer, _
                numPairsBuffer, sizeOfNumPairsBuffer)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        'nxStatus_t _NXFUNC nxConvertFramesToSignalsSinglePoint (
        '                                   nxSessionRef_t SessionRef,
        '                                   void * FrameBuffer,
        '                                   u32 NumberOfBytesForFrames,
        '                                   f64 * ValueBuffer,
        '                                   u32 SizeOfValueBuffer,
        '                                   nxTimestamp_t * TimestampBuffer,
        '                                   u32 SizeOfTimestampBuffer);/* 

        'public unsafe int nx_ConvertFramesToSignalsSinglePoint(void* frameBuffer, uint numberOfBytesForFrames, out double valueBuffer, uint sizeOfValueBuffer, out ulong timestampBuffer, uint sizeOfTimestampBuffer)
        '{
        '    int pInvokeResult = PInvoke.nx_convertFramesToSignalsSinglePoint(this._handle, frameBuffer, numberOfBytesForFrames, out valueBuffer, sizeOfValueBuffer, out timestampBuffer, sizeOfTimestampBuffer);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        'nxStatus_t _NXFUNC nxConvertSignalsToFramesSinglePoint (
        '                                   nxSessionRef_t SessionRef,
        '                                   f64 * ValueBuffer,
        '                                   u32 SizeOfValueBuffer,
        '                                   void * Buffer,
        '                                   u32 SizeOfBuffer,
        '                                   u32 * NumberOfBytesReturned);/* 

        'public unsafe int nx_ConvertSignalsToFramesSinglePoint(out double valueBuffer, uint sizeOfValueBuffer, void* buffer, uint sizeOfBuffer, out uint numberOfBytesReturned)
        '{
        '    int pInvokeResult = PInvoke.nx_convertSignalsToFramesSinglePoint(this._handle, out valueBuffer, sizeOfValueBuffer, buffer, sizeOfBuffer, out numberOfBytesReturned);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        'nxStatus_t _NXFUNC nxBlink (
        '                                   nxSessionRef_t InterfaceRef,
        '                                   u32 Modifier);/* 

        'public int nx_Blink(uint modifier)
        '{
        '    int pInvokeResult = PInvoke.nx_blink(this._handle, modifier);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        'nxStatus_t _NXFUNC nxClear (
        '                                   nxSessionRef_t SessionRef); 

        Public Function nx_Clear() As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_clear(Me._handle)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            'We set the handle to null and we MUST set _disposed to true. This prevents the class destructor (Finalize) from trying to Clear the session again (in Dispose)
            'Another option would be to call Dispose instead of Clear here, but not possible because Dispose is a sub, not a function
            Me._handle = System.IntPtr.Zero
            Me._disposed = True
            Return pInvokeResult
        End Function

        'nxStatus_t _NXFUNC nxConnectTerminals (
        '                                   nxSessionRef_t SessionRef,
        '                                   const char * source,
        '                                   const char * destination);/* 

        'public int nx_ConnectTerminals(string source, string destination)
        '{
        '    int pInvokeResult = PInvoke.nx_connectTerminals(this._handle, source, destination);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        'nxStatus_t _NXFUNC nxDisconnectTerminals (
        '                                   nxSessionRef_t SessionRef,
        '                                   const char * source,
        '                                   const char * destination);/* 

        'public int nx_DisconnectTerminals(string source, string destination)
        '{
        '    int pInvokeResult = PInvoke.nx_disconnectTerminals(this._handle, source, destination);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        'nxStatus_t _NXFUNC nxFlush (
        '                                   nxSessionRef_t SessionRef);/* 

        Public Function nx_Flush() As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_flush(Me._handle)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        'nxStatus_t _NXFUNC nxStart (
        '                                   nxSessionRef_t SessionRef,
        '                                   u32 Scope);/* 

        Public Function nx_Start(ByVal scope As UInteger) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_start(Me._handle, scope)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        'nxStatus_t _NXFUNC nxStop (
        '                                   nxSessionRef_t SessionRef,
        '                                   u32 Scope);/* 

        Public Function nx_Stop(ByVal scope As UInteger) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_stop(Me._handle, scope)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        'void _NXFUNC nxStatusToString (
        '                                   nxStatus_t Status,
        '                                   u32 SizeofString,
        '                                   char * StatusDescription);/* 

        Public Function nx_StatusToString(ByVal status As Integer, ByVal sizeOfString As UInteger, ByVal statusDescription As System.Text.StringBuilder) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_statusToString(status, sizeOfString, statusDescription)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        'nxStatus_t _NXFUNC nxSystemOpen (
        '                                   nxSessionRef_t * SystemRef);/* 

        'public niXNET()
        '{
        '    int pInvokeResult = PInvoke.nx_systemOpen(out this._handle);
        '    PInvoke.TestForError(System.IntPtr.Zero, pInvokeResult);
        '    this._disposed = false;
        '}

        'nxStatus_t _NXFUNC nxSystemClose (
        '                                   nxSessionRef_t SystemRef);/* 

        'public int nx_SystemClose()
        '{
        '    int pInvokeResult = PInvoke.nx_systemClose(this._handle);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        'nxStatus_t _NXFUNC nxWait (
        '                                   nxSessionRef_t SessionRef,
        '                                   u32 Condition,
        '                                   u32 ParamIn,
        '                                   f64 Timeout,
        '                                   u32 * ParamOut);/* 

        Public Function nx_Wait(ByVal condition As UInteger, ByVal paramIn As UInteger, ByVal timeOut As Double, ByRef paramOut As UInteger) As Integer
            Dim pInvokeResult As Integer = PInvoke.nx_wait(Me._handle, condition, paramIn, timeOut, paramOut)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        '***********************************************************************
        '       F U N C T I O N   P R O T O T Y P E S  :  D A T A B A S E
        '***********************************************************************/

        '*nxStatus_t _NXFUNC nxdbOpenDatabase (
        '                   const char * DatabaseName,
        '                   nxDatabaseRef_t * DatabaseRef); /* */
        Public Function nxdb_OpenDatabase(ByVal databaseName As String, ByRef databaseRef As UInteger)
            Dim pInvokeResult As Integer = PInvoke.nxdb_openDatabase(databaseName, databaseRef)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        '*nxStatus_t _NXFUNC nxdbCloseDatabase (
        '                           nxDatabaseRef_t DatabaseRef,
        '                           u32 CloseAllRefs);/* */
        'public int nxdb_CloseDatabase(uint databaseRef, uint closeAllRefs)
        '{
        '    int pInvokeResult = PInvoke.nxdb_closeDatabase(databaseRef, closeAllRefs);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        '*nxStatus_t _NXFUNC nxdbCreateObject (
        '                           nxDatabaseRef_t ParentObjectRef,
        '                           u32 ObjectClass,
        '                           const char * ObjectName,
        '                           nxDatabaseRef_t * DbObjectRef);/* */
        'public int nxdb_CreateObject(uint parentObjectRef, uint objectClass, string objecName, out uint dbObjectRef)
        '{
        '    int pInvokeResult = PInvoke.nxdb_createObject(parentObjectRef, objectClass, objecName, out dbObjectRef);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        '*nxStatus_t _NXFUNC nxdbFindObject (
        '                           nxDatabaseRef_t ParentObjectRef,
        '                           u32 ObjectClass,
        '                           const char * ObjectName,
        '                           nxDatabaseRef_t * DbObjectRef);/* */
        Public Function nxdb_FindObject(ByVal parentObjectRef As UInteger, ByVal objectClass As UInteger, ByVal objectName As String, ByRef dbObjectRef As UInteger)
            Dim pInvokeResult As Integer = PInvoke.nxdb_findObject(parentObjectRef, objectClass, objectName, dbObjectRef)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        '*nxStatus_t _NXFUNC nxdbDeleteObject (
        '                           nxDatabaseRef_t DbObjectRef);/* */
        'public int nxdb_DeleteObject(uint dbObjectRef)
        '{
        '    int pInvokeResult = PInvoke.nxdb_deleteObject(dbObjectRef);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        '*nxStatus_t _NXFUNC nxdbSaveDatabase (
        '                           nxDatabaseRef_t DatabaseRef,
        '                           const char * DbFilepath); /* */
        'public int nxdb_SaveDatabase(uint databaseRef, string dbFilepath)
        '{
        '    int pInvokeResult = PInvoke.nxdb_saveDatabase(databaseRef, dbFilepath);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        '*nxStatus_t _NXFUNC nxdbGetProperty (
        '                           nxDatabaseRef_t DbObjectRef,
        '                           u32 PropertyID,
        '                           u32 PropertySize,
        '                           void * PropertyValue);/* */
        Public Function nxdb_GetProperty(ByVal dbObjectRef As UInteger, ByVal propertyID As UInteger, ByVal propertySize As UInteger, ByRef propertyValue As String)
            Dim propertyPtr As New System.Text.StringBuilder(CInt(propertySize))
            Dim pInvokeResult As Integer = PInvoke.nxdb_getProperty(dbObjectRef, propertyID, propertySize, propertyPtr)
            'PInvoke.TestForError(Me._handle, pInvokeResult)
            propertyValue = propertyPtr.ToString()
            Return pInvokeResult
        End Function

        Public Function nxdb_GetProperty(ByVal dbObjectRef As UInteger, ByVal propertyID As UInteger, ByVal propertySize As UInteger, ByRef propertyValue As UInteger()) As Integer
            Dim pInvokeResult As Integer = PInvoke.nxdb_getProperty(dbObjectRef, propertyID, propertySize, propertyValue)
            PInvoke.TestForError(dbObjectRef, pInvokeResult)
            Return pInvokeResult
        End Function

        Public Function nxdb_GetProperty(ByVal dbObjectRef As UInteger, ByVal propertyID As UInteger, ByVal propertySize As UInteger, ByRef propertyValue As UInteger) As Integer
            Dim pInvokeResult As Integer = PInvoke.nxdb_getProperty(dbObjectRef, propertyID, propertySize, propertyValue)
            'PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function


        '*nxStatus_t _NXFUNC nxdbGetPropertySize (
        '                           nxDatabaseRef_t DbObjectRef,
        '                           u32 PropertyID,
        '                           u32 * PropertySize);/* */
        Public Function nxdb_GetPropertySize(ByVal dbObjectRef As UInteger, ByVal propertyID As UInteger, ByRef propertySize As UInteger)
            Dim pInvokeResult As Integer = PInvoke.nxdb_getPropertySize(dbObjectRef, propertyID, propertySize)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        '*nxStatus_t _NXFUNC nxdbSetProperty (
        '                           nxDatabaseRef_t DbObjectRef,
        '                           u32 PropertyID,
        '                           u32 PropertySize,
        '                           void * PropertyValue);/* */
        Public Function nxdb_SetProperty(ByVal dbObjectRef As UInteger, ByVal propertyID As UInteger, ByVal propertySize As UInteger, ByVal propertyValue As System.IntPtr) As Integer
            Dim pInvokeResult As Integer = PInvoke.nxdb_setProperty(dbObjectRef, propertyID, propertySize, propertyValue)
            PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        '* The NI-XNET documentation does not describe the Mode parameter.
        '   The Mode parameter was added near release. It specifies the type of DBC attribute
        '   that you want to search for. In the v1.2 release there is only one value:
        '      nxGetDBCMode_Attribute (0)
        '   Other values will be supported in subsequent releases.*/
        '*nxStatus_t _NXFUNC nxdbGetDBCAttributeSize (
        '                           nxDatabaseRef_t DbObjectRef,
        '                           u32 Mode,
        '                           const char* AttributeName,
        '                           u32 *AttributeTextSize);/* */
        'public int nxdb_GetDBCAttributeSize(uint dbObjectRef, uint mode, string attributeName, out uint attributeTextSize)
        '{
        '    int pInvokeResult = PInvoke.nxdb_getDBCAttributeSize(dbObjectRef, mode, attributeName, out attributeTextSize);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        '* The NI-XNET documentation does not describe the Mode parameter.
        '   The Mode parameter was added near release. It specifies the type of DBC attribute
        '   that you want to search for. In the v1.2 release there is only one value:
        '      nxGetDBCMode_Attribute (0)
        '   Other values will be supported in subsequent releases. */
        '*nxStatus_t _NXFUNC nxdbGetDBCAttribute (
        '                           nxDatabaseRef_t DbObjectRef,
        '                           u32 Mode,
        '                           const char* AttributeName,
        '                           u32 AttributeTextSize,
        '                           char* AttributeText,
        '                           u32 * IsDefault);/* */
        'public int nxdb_GetDBCAttribute(uint dbObjectRef, uint mode, string attributeName, uint attributeTextSize, out string attributeText, out uint isDefault)
        '{
        '    int pInvokeResult = PInvoke.nxdb_getDBCAttribute(dbObjectRef, mode, attributeName, attributeTextSize, out attributeText, out isDefault);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        '*nxStatus_t _NXFUNC nxdbAddAlias (
        '                           const char * DatabaseAlias,
        '                           const char * DatabaseFilepath,
        '                           u32          DefaultBaudRate);/* */
        'public int nxdb_AddAlias(string databaseAlias, string databaseFilepath, uint defaultBaudRate)
        '{
        '    int pInvokeResult = PInvoke.nxdb_addAlias(databaseAlias, databaseFilepath, defaultBaudRate);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        '*nxStatus_t _NXFUNC nxdbRemoveAlias (
        '                           const char * DatabaseAlias);/* */
        'public int nxdb_RemoveAlias(string databaseAlias)
        '{
        '    int pInvokeResult = PInvoke.nxdb_removeAlias(databaseAlias);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        '*nxStatus_t _NXFUNC nxdbDeploy (
        '                           const char * IPAddress,
        '                           const char * DatabaseAlias,
        '                           u32 WaitForComplete,
        '                           u32 * PercentComplete);/* */
        'public int nxdb_Deploy(string ipAddress, string databaseAlias, uint waitForComplete, out uint percentComplete)
        '{
        '    int pInvokeResult = PInvoke.nxdb_deploy(ipAddress, databaseAlias, waitForComplete, out percentComplete);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        '*nxStatus_t _NXFUNC nxdbUndeploy (
        '                           const char * IPAddress,
        '                           const char * DatabaseAlias);/* */
        'public int nxdb_Undeploy(string ipAddress, string databaseAlias)
        '{
        '    int pInvokeResult = PInvoke.nxdb_undeploy(ipAddress, databaseAlias);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        '*nxStatus_t _NXFUNC nxdbGetDatabaseList (
        '                           const char * IPAddress,
        '                           u32 SizeofAliasBuffer,
        '                           char * AliasBuffer,
        '                           u32 SizeofFilepathBuffer,
        '                           char * FilepathBuffer,
        '                           u32 * NumberOfDatabases);/* */
        'public int nxdb_GetDatabaseList(string ipAddress, uint sizeofAliasBuffer, out string aliasBuffer, uint sizeofFilepathBuffer, out string filepathBuffer, out uint numberOfDatabases)
        '{
        '    int pInvokeResult = PInvoke.nxdb_getDatabaseList(ipAddress, sizeofAliasBuffer, out aliasBuffer, sizeofFilepathBuffer, out filepathBuffer, out numberOfDatabases);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        '*nxStatus_t _NXFUNC nxdbGetDatabaseListSizes (
        '                           const char * IPAddress,
        '                           u32 * SizeofAliasBuffer,
        '                           u32 * SizeofFilepathBuffer);/* */
        'public int nxdb_GetDatabaseListSizes(string ipAddress, out uint sizeofAliasBuffer, out uint sizeofFilepathBuffer)
        '{
        '    int pInvokeResult = PInvoke.nxdb_getDatabaseListSizes(ipAddress, out sizeofAliasBuffer, out sizeofFilepathBuffer);
        '    PInvoke.TestForError(this._handle, pInvokeResult);
        '    return pInvokeResult;
        '}

        Public Class xNETConstants
            Public Const NX_STATUS_QUALIFIER As Integer = &H3FF63000
            Public Const NX_STATUS_WARNING As Integer = &H0
            Public Const NX_STATUS_ERROR As Integer = &H80000000
            Public Const NX_WARNING_BASE As Integer = (NX_STATUS_QUALIFIER Or NX_STATUS_WARNING)
            Public Const NX_ERROR_BASE As Integer = (NX_STATUS_QUALIFIER Or NX_STATUS_ERROR)

            Public Const nxSuccess As Integer = 0

            ' Macros to get fields of uint returned by nxReadState of nxState_SessionInfo
            ' Get state of frames in the session; uses constants with prefix nxSessionInfoState_
            Public Function nxSessionInfo_Get_State(ByVal StateValue As UInteger) As Byte
                Return CByte(CUInt(StateValue) And &H3)
            End Function

            ' Macros to get fields of uint returned by nxReadState of nxState_CANComm
            ' Get CAN communication state; uses constants with prefix nxCANCommState_
            Public Function nxCANComm_Get_CommState(ByVal StateValue As UInteger) As Byte
                Return CByte(CUInt(StateValue) And &HF)
            End Function
            ' Get CAN transceiver error (!NERR); 1 = error, 0 = no error
            Public Function nxCANComm_Get_TcvrErr(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 4) And &H1)
            End Function
            ' Get indication of CAN controller/transceiver sleep; 1 = asleep, 0 = awake
            Public Function nxCANComm_Get_Sleep(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 5) And &H1)
            End Function
            ' Get last bus error that incremented counters; uses constants with prefix nxCANLastErr_
            Public Function nxCANComm_Get_LastErr(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 8) And &HF)
            End Function
            ' Get Transmit Error Counter as defined by the CAN protocol specification
            Public Function nxCANComm_Get_TxErrCount(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 16) And &HFF)
            End Function

            ' Get Receive Error Counter as defined by the CAN protocol specification
            Public Function nxCANComm_Get_RxErrCount(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 24) And &HFF)
            End Function


            Public Const nxErrInternalError As Integer = (NX_ERROR_BASE Or &H1)

            '! Board self test failed(code 2). Solution: try reinstalling the driver or
            '! switching the slot(s) of the board(s). If the error persists,contact
            '! National Instruments.
            Public Const nxErrSelfTestError1 As Integer = (NX_ERROR_BASE Or &H2)

            '! Board self test failed(code 3). Solution: try reinstalling the driver or
            '! switching the slot(s) of the board(s). If the error persists,contact
            '! National Instruments.
            Public Const nxErrSelfTestError2 As Integer = (NX_ERROR_BASE Or &H3)

            '! Board self test failed(code 4). Solution: try reinstalling the driver or
            '! switching the slot(s) of the board(s). If the error persists,contact
            '! National Instruments.
            Public Const nxErrSelfTestError3 As Integer = (NX_ERROR_BASE Or &H4)

            '! Board self test failed(code 5). Solution: try reinstalling the driver or
            '! switching the slot(s) of the board(s). If the error persists,contact
            '! National Instruments.
            Public Const nxErrSelfTestError4 As Integer = (NX_ERROR_BASE Or &H5)

            '! Board self test failed(code 6). Solution: try reinstalling the driver or
            '! switching the slot(s) of the board(s). If the error persists,contact
            '! National Instruments.
            Public Const nxErrSelfTestError5 As Integer = (NX_ERROR_BASE Or &H6)

            '! Computer went to hibernation mode and the board lost power. Solution:
            '! prevent the computer from going to hibernation mode in the control panel.
            Public Const nxErrPowerSuspended As Integer = (NX_ERROR_BASE Or &H7)

            '! A write queue overflowed. Solution: wait until queue space becomes available
            '! and retry.
            Public Const nxErrOutputQueueOverflow As Integer = (NX_ERROR_BASE Or &H8)

            '! The board's firmware did not answer a command. Solution: Stop your
            '! application and execute a self test. Try deactivating/reactivating the
            '! driver in the Device Manager. If the problem persists, contact National
            '! Instruments.
            Public Const nxErrFirmwareNoResponse As Integer = (NX_ERROR_BASE Or &H9)

            '! The operation timed out. Solution: specify a timeout long enough to complete
            '! the operation, or change the operation in a way that it can get completed in
            '! less time (e.g. read less data).
            Public Const nxErrEventTimeout As Integer = (NX_ERROR_BASE Or &HA)

            '! A read queue overflowed. Solution: reduce your data rate or call Read more
            '! frequently.
            Public Const nxErrInputQueueOverflow As Integer = (NX_ERROR_BASE Or &HB)

            '! The Read buffer is too small to hold a single frame. Solution: provide a
            '! buffer large enough.
            Public Const nxErrInputQueueReadSize As Integer = (NX_ERROR_BASE Or &HC)

            '! You tried to open the same frame twice. This is not permitted. Solution:
            '! open each frame only once.
            Public Const nxErrDuplicateFrameObject As Integer = (NX_ERROR_BASE Or &HD)

            '! You tried to open the same stream object twice. This is not permitted.
            '! Solution: open each stream object only once.
            Public Const nxErrDuplicateStreamObject As Integer = (NX_ERROR_BASE Or &HE)

            '! Self test is not possible since the board is in use by an application.
            '! Solution: stop all NI-XNET applications before executing a self test.
            Public Const nxErrSelfTestNotPossible As Integer = (NX_ERROR_BASE Or &HF)

            '! Allocation of memory failed. You do not have sufficient memory in the
            '! LabVIEW target. Solution: add more RAM or try to use fewer resources in your
            '! applications (arrays, XNET sessions, etc).
            Public Const nxErrMemoryFull As Integer = (NX_ERROR_BASE Or &H10)

            '! The maximum number of sessions was exceeded. Solution: use fewer sessions.
            Public Const nxErrMaxSessions As Integer = (NX_ERROR_BASE Or &H11)

            '! The maximum number of frames has been exceeded. Solution: Use fewer frames
            '! in your sessions.
            Public Const nxErrMaxFrames As Integer = (NX_ERROR_BASE Or &H12)

            '! The maximum number of devices has been detected. Solution: use fewer
            '! devices.
            Public Const nxErrMaxDevices As Integer = (NX_ERROR_BASE Or &H13)

            '! A driver support file is missing. Solution: try reinstalling the driver. If
            '! the error persists, contact National Instruments.
            Public Const nxErrMissingFile As Integer = (NX_ERROR_BASE Or &H14)

            '! This indicates that a NULL pointer or an empty string was passed to a
            '! function. The user should verify that the parameters passed in make sense
            '! for the given function.
            Public Const nxErrParameterNullOrEmpty As Integer = (NX_ERROR_BASE Or &H15)

            '! The maximum number of schedules has been detected. Solution: Use fewer
            '! schedules.
            Public Const nxErrMaxSchedules As Integer = (NX_ERROR_BASE Or &H16)

            '! Board self test failed (code 17). Solution: Try reinstalling the driver or
            '! switching the slot(s) of the board(s). If the error persists, contact
            '! National Instruments.
            Public Const nxErrSelfTestError6 As Integer = (NX_ERROR_BASE Or &H17)

            '! You cannot start an NI-XNET application while a self test is in progress.
            '! Solution: Complete the self test before starting any NI-XNET applications.
            Public Const nxErrSelfTestInProgress As Integer = (NX_ERROR_BASE Or &H18)

            '! An invalid reference has been passed to a NI-XNET session function.
            '! Solution: Only pass reference retrieved from Create Session, or from an IO
            '! name of a session in LabVIEW project.
            Public Const nxErrInvalidSessionHandle As Integer = (NX_ERROR_BASE Or &H20)

            '! An invalid reference has been passed to a NI-XNET system function. Solution:
            '! Only pass a valid system reference.
            Public Const nxErrInvalidSystemHandle As Integer = (NX_ERROR_BASE Or &H21)

            '! A device reference was expected for a NI-XNET session function. Solution:
            '! Only pass a device reference.
            Public Const nxErrDeviceHandleExpected As Integer = (NX_ERROR_BASE Or &H22)

            '! An interface reference was expected for a NI-XNET session function.
            '! Solution: Only pass an interface reference.
            Public Const nxErrIntfHandleExpected As Integer = (NX_ERROR_BASE Or &H23)

            '! You have configured a property that conflicts with the current mode of the
            '! session. For example, you have created a CAN output session with a frame
            '! configured with a Timing Type = Cyclic and a Transmit Time of 0.
            Public Const nxErrPropertyModeConflicting As Integer = (NX_ERROR_BASE Or &H24)

            '! XNET Create Timing Source VI is not supported on Windows. This VI is
            '! supported on LabVIEW Real-Time targets only.
            Public Const nxErrTimingSourceNotSupported As Integer = (NX_ERROR_BASE Or &H25)

            '! You tried to create more than one LabVIEW timing source for a single
            '! interface. Only one timing source per interface is supported. The timing
            '! source remains until the top-level VI is idle (no longer running). Solution:
            '! Call the XNET Create Timing Source VI only once per interface. You can use
            '! the timing source with multiple timed structures (e.g. timed loops).
            Public Const nxErrMultipleTimingSource As Integer = (NX_ERROR_BASE Or &H26)

            '! You invoked two or more VIs simultaneously for the same session, and those
            '! VIs do not support overlap. For example, you attempted to invoke two Read
            '! VIs at the same time for the same session. Solution: Wire the error cluster
            '! from one VI to another, to enforce sequential execution for the session.
            Public Const nxErrOverlappingIO As Integer = (NX_ERROR_BASE Or &H27)

            '! You are trying to start an interface that is missing bus power for the
            '! transceiver. Some physical layers on NI-XNET hardware are internally
            '! powered, but others require external power for the port to operate. This
            '! error occurs when starting an interface on hardware that requires external
            '! power when no power is detected. Solution: Supply proper voltage to your
            '! transceiver. Refer to the NI-XNET Hardware Overview in the NI-XNET Hardware
            '! and Software Manual for more information.
            Public Const nxErrMissingBusPower As Integer = (NX_ERROR_BASE Or &H28)

            '! The connection with a CompactDAQ chassis was lost, and the host software and
            '! modules are out of sync. There is no direct recovery for this problem until
            '! the chassis is reset. Solutions: Call DAQmx Reset Device as the first VI or
            '! function in your application, prior to creating XNET sessions. Alternately,
            '! you could reset the CompactDAQ chassis in Measurement and Automation
            '! Explorer (MAX).
            Public Const nxErrCdaqConnectionLost As Integer = (NX_ERROR_BASE Or &H29)

            '! The transceiver value set is invalid (for this port, e.g. LS on a HS port)
            '! or you are trying to perform an operation that requires a different
            '! transceiver (e.g., trying to change the state of a disconnected
            '! transceiver). Solution: set a valid value.
            Public Const nxErrInvalidTransceiver As Integer = (NX_ERROR_BASE Or &H71)

            '! The baud rate value set is invalid. Solution: set a valid value.
            Public Const nxErrInvalidBaudRate As Integer = (NX_ERROR_BASE Or &H72)

            '! No baud rate value has been set. Solution: set a valid value.
            Public Const nxErrBaudRateNotConfigured As Integer = (NX_ERROR_BASE Or &H73)

            '! The bit timing values set are invalid. Solution: set valid values.
            Public Const nxErrInvalidBitTimings As Integer = (NX_ERROR_BASE Or &H74)

            '! The baud rate set does not match the transceiver's allowed range. Solution:
            '! change either the baud rate or the transceiver.
            Public Const nxErrBaudRateXcvrMismatch As Integer = (NX_ERROR_BASE Or &H75)

            '! The configured terminal is not known for this interface. Solution: Make sure
            '! that that you pass in a valid value to Connect Terminals or Disconnect
            '! Terminals.
            Public Const nxErrUnknownTimingSource As Integer = (NX_ERROR_BASE Or &H76)

            '! The configured terminal is inappropriate for the hardware. For example,
            '! setting a source to FrontPanel0 on XNET hardware that doesn't have
            '! front-panel trigger inputs, or selecting PXI_Clk10 for a non-PXI device.
            '! Solution: Pick an appropriate terminal for the hardware.
            Public Const nxErrUnknownSynchronizationSource As Integer = (NX_ERROR_BASE Or &H77)

            '! The source that you connected to the Master Timebase destination is missing.
            '! When the start trigger is received, the interface verifies that a signal is
            '! present on the configured source. This check has determined that this signal
            '! is missing. Solution: Verify that your cables are configured correctly and
            '! that your timebase source is generating an appropriate waveform.
            Public Const nxErrMissingTimebaseSource As Integer = (NX_ERROR_BASE Or &H78)

            '! The source that you connected to the Master Timebase destination is not
            '! generating an appropriate signal. When the start trigger is received, the
            '! interface verifies that a signal of a known frequency is present on the
            '! configured source. This check has determined that this source is generating
            '! a signal, but that the signal is not one of the supported frequencies for
            '! this hardware. Solution: Verify that your source is generating a signal at a
            '! supported frequency.
            Public Const nxErrUnknownTimebaseFrequency As Integer = (NX_ERROR_BASE Or &H79)

            '! You are trying to disconnect a synchronization terminal that is not
            '! currently connected. Solution: Only disconnect synchronization terminals
            '! that have previously been connected.
            Public Const nxErrUnconnectedSynchronizationSource As Integer = (NX_ERROR_BASE Or &H7A)

            '! You are trying to connect a synchronization terminal that is already in use.
            '! For example, you are trying to connect a trigger line to the Master Timebase
            '! when a different trigger line is already connected to the Master Timebase.
            '! Solution: Only connect to synchronization terminals that are not currently
            '! in use.
            Public Const nxErrConnectedSynchronizationTerminal As Integer = (NX_ERROR_BASE Or &H7B)

            '! You are trying to connect an XNET terminal as a source terminal, but the
            '! desired XNET terminal is not valid as a source terminal. Solution: Only
            '! connect valid source terminals to the source terminal in XNET Connect
            '! Terminals.
            Public Const nxErrInvalidSynchronizationSource As Integer = (NX_ERROR_BASE Or &H7C)

            '! You are trying to connect an XNET terminal as a destination terminal, but
            '! the desired XNET terminal is not valid as a destination terminal. Solution:
            '! Only connect valid destination terminals to the destination terminal in XNET
            '! Connect Terminals.
            Public Const nxErrInvalidSynchronizationDestination As Integer = (NX_ERROR_BASE Or &H7D)

            '! You are trying to connect two XNET terminals that are incompatible.
            '! Solution: Only connect a source and destination terminals that are
            '! compatible with each other.
            Public Const nxErrInvalidSynchronizationCombination As Integer = (NX_ERROR_BASE Or &H7E)

            '! The source that you connected to the Master Timebase destination has
            '! disappeared. When the start trigger is received, the interface verifies that
            '! a signal is present on the configured source. This check has determined that
            '! this signal was present, but while the interface was running, the signal
            '! disappeared, so all timebase configuration has reverted to using the onboard
            '! (unsynchronized) oscillator. Solution: Verify that your cables are
            '! configured correctly and that your timebase source is generating an
            '! appropriate waveform the entire time your application is running.
            Public Const nxErrTimebaseDisappeared As Integer = (NX_ERROR_BASE Or &H7F)

            '! You called Read (State : FlexRay : Cycle Macrotick), and the FlexRay
            '! Macrotick is not connected as the master timebase of the interface.
            '! Solution: Call Connect Terminals to connect source of FlexRay Macrotick to
            '! destination of Master Timebase.
            Public Const nxErrMacrotickDisconnected As Integer = (NX_ERROR_BASE Or &H80)

            '! The database specified could not be opened. Solution: Check that the alias
            '! and/or the file exist and that it is a valid database.
            Public Const nxErrCannotOpenDatabaseFile As Integer = (NX_ERROR_BASE Or &H81)

            '! The cluster was not found in the database. Solution: Make sure you only
            '! initialize a cluster in a session that is defined in the database.
            Public Const nxErrClusterNotFound As Integer = (NX_ERROR_BASE Or &H82)

            '! The frame was not found in the database. Solution: Make sure you only
            '! initialize frames in a session that are defined in the database.
            Public Const nxErrFrameNotFound As Integer = (NX_ERROR_BASE Or &H83)

            '! The signal was not found in the database. Solution: Make sure you only
            '! initialize signals in a session that are defined in the database.
            Public Const nxErrSignalNotFound As Integer = (NX_ERROR_BASE Or &H84)

            '! A necessary property for a cluster was not found in the database. Solution:
            '! Make sure you only initialize a cluster in a session that is completely
            '! defined in the database.
            Public Const nxErrUnconfiguredCluster As Integer = (NX_ERROR_BASE Or &H85)

            '! A necessary property for a frame was not found in the database. Solution:
            '! Make sure you only initialize frames in a session that are completely
            '! defined in the database.
            Public Const nxErrUnconfiguredFrame As Integer = (NX_ERROR_BASE Or &H86)

            '! A necessary property for a signal was not found in the database. Solution:
            '! Make sure you only initialize signals in a session that are completely
            '! defined in the database.
            Public Const nxErrUnconfiguredSignal As Integer = (NX_ERROR_BASE Or &H87)

            '! Multiple clusters have been specified in one session, either directly
            '! (Stream I/O), or through the signals or frames specified. Solution: Make
            '! sure that in one session, you open only one cluster, including frames or
            '! signals that belong to the same cluster.
            Public Const nxErrMultipleClusters As Integer = (NX_ERROR_BASE Or &H88)

            '! You specified a database of ':subordinate:' for a session mode other than
            '! mode of Frame Input Stream. Solution: either open a Frame Input Stream
            '! session, or use a real or in-memory database.
            Public Const nxErrSubordinateNotAllowed As Integer = (NX_ERROR_BASE Or &H89)

            '! The interface name given does not specify a valid and existing interface.
            '! Solution: Use a valid and existing interface. These can be obtained using
            '! MAX, XNET system properties, or the LabVIEW XNET Interface IO name. If you
            '! are using CompactRIO, refer to the topic "Getting Started with CompactRIO"
            '! in the NI-XNET Hardware and Software Help.
            Public Const nxErrInvalidInterface As Integer = (NX_ERROR_BASE Or &H8A)

            '! The operation is invalid for this interface (e.g. you tried to open a set of
            '! FlexRay frames on a CAN interface, or tried to request a CAN property from a
            '! FlexRay interface). Solution: run this operation on a suitable interface.
            Public Const nxErrInvalidProtocol As Integer = (NX_ERROR_BASE Or &H8B)

            '! You tried to set the AutoStart property to FALSE for an Input session. This
            '! is not allowed. Solution: don't set the AutoStart property (TRUE is
            '! default).
            Public Const nxErrInputSessionMustAutoStart As Integer = (NX_ERROR_BASE Or &H8C)

            '! The property ID you specified is not valid (or not valid for the current
            '! session mode or form factor).
            Public Const nxErrInvalidPropertyId As Integer = (NX_ERROR_BASE Or &H8D)

            '! The contents of the property is bigger than the size specified. Use the
            '! nxGetPropertySize function to determine the size of the buffer needed.
            Public Const nxErrInvalidPropertySize As Integer = (NX_ERROR_BASE Or &H8E)

            '! The function you called is not defined for the session mode (e.g. you called
            '! a frame I/O function on a signal I/O session).
            Public Const nxErrIncorrectMode As Integer = (NX_ERROR_BASE Or &H8F)

            '! The data that you passed to the XNET Write is too small to hold all the data
            '! specified for the session. Solution: determine the number of elements
            '! (frames or signals) that you configured for the session, and pass that
            '! number of elements to XNET Write.
            Public Const nxErrBufferTooSmall As Integer = (NX_ERROR_BASE Or &H90)

            '! For Signal Output sessions, the multiplexer signals used in the session must
            '! be specified explicitly in the signal list.
            Public Const nxErrMustSpecifyMultiplexers As Integer = (NX_ERROR_BASE Or &H91)

            '! You used an XNET Session IO name, and that session was not found in your
            '! LabVIEW project. Solution: Within LabVIEW project, right-click the target
            '! (RT or My Computer), and select New > NI-XNET Session. Add the VI that uses
            '! the session under the target. If you are using the session with a built
            '! application (.EXE), ensure that you copy the built configuration file
            '! nixnetSession.txt such that it resides in the same folder as the executable.
            Public Const nxErrSessionNotFound As Integer = (NX_ERROR_BASE Or &H92)

            '! You used the same XNET session name in multiple top-level VIs, which is not
            '! supported. Solution: Use each session in only one top-level VI (application)
            '! at a time.
            Public Const nxErrMultipleUseOfSession As Integer = (NX_ERROR_BASE Or &H93)

            '! To execute this function properly, the session's list must contain only one
            '! frame. Solution: break your session up into multiple, each of which contains
            '! only one frame.
            Public Const nxErrOnlyOneFrame As Integer = (NX_ERROR_BASE Or &H94)

            '! You used the same alias for different database files which is not allowed.
            '! Solution: Use each alias only for a single database file.
            Public Const nxErrDuplicateAlias As Integer = (NX_ERROR_BASE Or &H95)

            '! You try to deploy a database file while another deployment is in progress.
            '! Solution: wait until the other deployment has finished and try again.
            Public Const nxErrDeploymentInProgress As Integer = (NX_ERROR_BASE Or &H96)

            '! A signal or frame session has been opened, but it doesn’t contain signals or
            '! frames. Solution: specify at least one signal or frame.
            Public Const nxErrNoFramesOrSignals As Integer = (NX_ERROR_BASE Or &H97)

            '! An invalid value has been specified for the ‘mode?parameter. Solution:
            '! specify a valid value.
            Public Const nxErrInvalidMode As Integer = (NX_ERROR_BASE Or &H98)

            '! A session was created by references, but no database references have been
            '! specified. Solution: specify at least one appropriate database reference
            '! (i.e. signal or frame or cluster ref depending on the session mode).
            Public Const nxErrNeedReference As Integer = (NX_ERROR_BASE Or &H99)

            '! The interface has already been opened with different cluster settings than
            '! the ones specified for this session. Solution: make sure that the cluster
            '! settings agree for the interface, or use a different interface.
            Public Const nxErrDifferentClusterOpen As Integer = (NX_ERROR_BASE Or &H9A)

            '! The cycle repetition of a frame in the database for the FlexRay protocol is
            '! invalid. Solution: Make sure that the cycle repetition is a power of 2
            '! between 1 and 64.
            Public Const nxErrFlexRayInvalidCycleRep As Integer = (NX_ERROR_BASE Or &H9B)

            '! You called XNET Clear for the session, then tried to perform another
            '! operation. Solution: Defer clear (session close) until you are done using
            '! it. This error can also occur if you branch a wire after creating the
            '! session. Solution: Do not branch a session to multiple flows in the diagram.
            Public Const nxErrSessionCleared As Integer = (NX_ERROR_BASE Or &H9C)

            '! You called Create Session VI with a list of items that does not match the
            '! mode. This includes using: 1) signal items for a Frame I/O mode 2) frame
            '! items for a Signal I/O mode 3) cluster item for a mode other than Frame
            '! Input Stream or Frame Output Stream
            Public Const nxErrWrongModeForCreateSelection As Integer = (NX_ERROR_BASE Or &H9D)

            '! You tried to create a new session while the interface is already running.
            '! Solution: Create all sessions before starting any of them.
            Public Const nxErrInterfaceRunning As Integer = (NX_ERROR_BASE Or &H9E)

            '! You wrote a frame whose payload length is larger than the maximum payload
            '! allowed by the database (e.g. wrote 10 bytes for CAN frame, max 8 bytes).
            '! Solution: Never write more payload bytes than the Payload Length Maximum
            '! property of the session.
            Public Const nxErrFrameWriteTooLarge As Integer = (NX_ERROR_BASE Or &H9F)

            '! You called a Read function with a nonzero timeout, and you used a negative
            '! numberToRead. Negative value for numberToRead requests all available data
            '! from the Read, which is ambiguous when used with a timeout. Solutions: 1)
            '! Pass timeout of and numberToRead of -1, to request all available data. 2)
            '! Pass timeout > 0, and numberToRead > 0, to wait for a specific number of
            '! data elements.
            Public Const nxErrTimeoutWithoutNumToRead As Integer = (NX_ERROR_BASE Or &HA0)

            '! Timestamps are not (yet) supported for Write Signal XY. Solution: Do not
            '! provide a timestamp array for Write Signal XY.
            Public Const nxErrTimestampsNotSupported As Integer = (NX_ERROR_BASE Or &HA1)

            ' \REVIEW: Rename to WaitCondition
            '! The condition parameter passed to Wait is not known. Solution: Pass a valid
            '! parameter.
            Public Const nxErrUnknownCondition As Integer = (NX_ERROR_BASE Or &HA2)

            '! You attempted an I/O operation, but the session is not yet started (and the
            '! AutoStart property is set to FALSE). Solution: call Start before you use
            '! this IO operation.
            Public Const nxErrSessionNotStarted As Integer = (NX_ERROR_BASE Or &HA3)

            '! The maximum number of Wait operations has been exceeded. Solution: If you
            '! are waiting for multiple events on the interface, use fewer Wait operations
            '! on this interface (even for multiple sessions). If you are waiting for
            '! multiple events for a frame (e.g. transmit complete), use only one Wait at a
            '! time for that frame.
            Public Const nxErrMaxWaitsExceeded As Integer = (NX_ERROR_BASE Or &HA4)

            '! You used an invalid name for an XNET Device. Solution: Get valid XNET Device
            '! names from the XNET System properties (only).
            Public Const nxErrInvalidDevice As Integer = (NX_ERROR_BASE Or &HA5)

            '! A terminal name passed to ConnectTerminals or DisconnectTerminals is
            '! unknown. Solution: only pass valid names.
            Public Const nxErrInvalidTerminalName As Integer = (NX_ERROR_BASE Or &HA6)

            '! You tried to blink the port LEDs but these are currently busy. Solution:
            '! stop all applications running on that port; do not access it from MAX or LV
            '! Project.
            Public Const nxErrPortLEDsBusy As Integer = (NX_ERROR_BASE Or &HA7)

            '! You tried to set a FlexRay keyslot ID that is not listed as valid in the
            '! database. Solution: only pass slot IDs of frames that have the startup or
            '! sync property set in the database.
            Public Const nxErrInvalidKeyslot As Integer = (NX_ERROR_BASE Or &HA8)

            '! You tried to set a queue size that is bigger than the maximum allowed.
            '! Solution: Specify an in-range queue size.
            Public Const nxErrMaxQueueSizeExceeded As Integer = (NX_ERROR_BASE Or &HA9)

            '! You wrote a frame whose payload length is different than the payload length
            '! configured by the database. Solution: Never write a different payload length
            '! for a frame that is different than the configured payload length.
            Public Const nxErrFrameSizeMismatch As Integer = (NX_ERROR_BASE Or &HAA)

            '! The index to indicate an session list element is too large. Solution:
            '! Specify an index in the range ... NumInList-1.
            Public Const nxErrIndexTooBig As Integer = (NX_ERROR_BASE Or &HAB)

            '! You have tried to create a session that is invalid for the mode of the
            '! driver/firmware. For example, you are using the Replay Exclusive mode for
            '! Stream Output and you have an output session open.
            Public Const nxErrSessionModeIncompatibility As Integer = (NX_ERROR_BASE Or &HAC)

            '! You have tried to create a session using a frame that is incompatible with
            '! the selected session type. For example, you are using a LIN diagnostic frame
            '! with a single point output session.
            Public Const nxErrSessionTypeFrameIncompatibility As Integer = (NX_ERROR_BASE Or &H15D)

            '! The trigger signal for a frame is allowed only in Single Point Signal
            '! sessions (Input or Output). For Output Single Point Signal Sessions only one
            '! trigger signal is allowed per frame. Solution: Do not use the trigger
            '!signal, or change to a single point I/O session.
            Public Const nxErrTriggerSignalNotAllowed As Integer = (NX_ERROR_BASE Or &HAD)

            '! To execute this function properly, the session's list must contain only one
            '! cluster. Solution: Use only one cluster in the session.
            Public Const nxErrOnlyOneCluster As Integer = (NX_ERROR_BASE Or &HAE)

            '! You attempted to convert a CAN or LIN frame with a payload length greater
            '! than 8. For example, you may be converting a frame that uses a higher layer
            '! transport protocol, such as SAE-J1939. NI-XNET currently supports conversion
            '! of CAN/LIN frames only (layer 2). Solutions: 1) Implement higher layer
            '! protocols (including signal conversion) within your code. 2) Contact
            '! National Instruments to request this feature in a future version.
            Public Const nxErrConvertInvalidPayload As Integer = (NX_ERROR_BASE Or &HAF)

            '! Allocation of memory failed for the data returned from LabVIEW XNET Read.
            '! Solutions: 1) Wire a smaller "number to read" to XNET Read (default -1 uses
            '! queue size). 2) For Signal Input Waveform, use a smaller resample rate. 3)
            '! Set smaller value for session's queue size property (default is large to
            '! avoid loss of data).
            Public Const nxErrMemoryFullReadData As Integer = (NX_ERROR_BASE Or &HB0)

            '! Allocation of memory failed in the firmware. Solutions: 1) Create less
            '! firmware objects 2) Set smaller value for output session's queue size
            '! property (default is large to avoid loss of data).
            Public Const nxErrMemoryFullFirmware As Integer = (NX_ERROR_BASE Or &HB1)

            '! The NI-XNET driver no longer can communicate with the device. Solution: Make
            '! sure the device has not been removed from the computer.
            Public Const nxErrCommunicationLost As Integer = (NX_ERROR_BASE Or &HB2)

            '! A LIN schedule has an invalid priority. Solution: Use a valid priority (0 =
            '! NULL schedule, 1..254 = Run once schedule, 255 = Continuous schedule).
            Public Const nxErrInvalidPriority As Integer = (NX_ERROR_BASE Or &HB3)

            '! (Dis)ConnectTerminals is not allowed for XNET C Series modules. Solution: To
            '! connect the module start trigger, use the Session property Interface Source
            '! Terminal Start Trigger.
            Public Const nxErrSynchronizationNotAllowed As Integer = (NX_ERROR_BASE Or &HB4)

            '! You requested a time (like Start or Communication Time) before the event has
            '! happened. Solution: Request the time only after it occurred.
            Public Const nxErrTimeNotReached As Integer = (NX_ERROR_BASE Or &HB5)

            '! An internal input queue overflowed. Solution: Attempt to pull data from the
            '! hardware faster. If you are connected by an external bus (for example, USB
            '! or Ethernet), you can try to use a faster connection.
            Public Const nxErrInternalInputQueueOverflow As Integer = (NX_ERROR_BASE Or &HB6)

            '! A bad firmware image file can not be loaded to the hardware. Solution:
            '! Uninstall and reinstall the NI-XNET software as the default firmware file
            '! may be corrupt. If you are using a custom firmware file, try rebuilding it.
            Public Const nxErrBadImageFile As Integer = (NX_ERROR_BASE Or &HB7)

            '! The encoding of embedded network data (CAN, FlexRay, LIN, etc.) within the
            '! TDMS file is invalid. Solutions: 1) In the application that wrote (created)
            '! the logfile, and the application in which you are reading it, confirm that
            '! both use the same major version for frame data encoding
            '! (NI_network_frame_version property of the TDMS channel). 2) Ensure that your
            '! file was not corrupted.
            Public Const nxErrInvalidLogfile As Integer = (NX_ERROR_BASE Or &HB8)

            '! A property value was out of range or incorrect. Solution: specify a correct
            '! value.
            Public Const nxErrInvalidPropertyValue As Integer = (NX_ERROR_BASE Or &HC0)

            '! Integration of the interface into the FlexRay cluster failed, so
            '! communication did not start for the interface. Solution: check the cluster
            '! and/or interface parameters and verify that there are startup frames
            '! defined.
            Public Const nxErrFlexRayIntegrationFailed As Integer = (NX_ERROR_BASE Or &HC1)

            '! The PDU was not found in the database. Solution: Make sure you initialize
            '! only PDUs in a session that are defined in the database.
            Public Const nxErrPduNotFound As Integer = (NX_ERROR_BASE Or &HD0)

            '! A necessary property for a PDU was not found in the database. Solution: Make
            '! sure you initialize only PDUs in a session that are completely defined in
            '! the database.
            Public Const nxErrUnconfiguredPdu As Integer = (NX_ERROR_BASE Or &HD1)

            '! You tried to open the same PDU twice. This is not permitted. Solution: Open
            '! each PDU only once.
            Public Const nxErrDuplicatePduObject As Integer = (NX_ERROR_BASE Or &HD2)

            '! You can access this database object only by PDU, not by frame. Solution: For
            '! CAN and LIN, this is not supported by the current version of NI-XNET; for
            '! FlexRay, make sure the database is set to use PDUs.
            Public Const nxErrNeedPdu As Integer = (NX_ERROR_BASE Or &HD3)

            '! Remote communication with the LabVIEW RT target failed. Solution: check if
            '! NI-XNET has been installed on the RT target and check if the NI-XNET RPC
            '! server has been started.
            Public Const nxErrRPCCommunication As Integer = (NX_ERROR_BASE Or &H100)

            '! File transfer communication with the LabVIEW Real-Time (RT) target failed.
            '! Solution: check if the RT target has been powered on, the RT target has been
            '! connected to the network, and if the IP address settings are correct.
            Public Const nxErrFileTransferCommunication As Integer = (NX_ERROR_BASE Or &H101)
            Public Const nxErrFTPCommunication As Integer = (NX_ERROR_BASE Or &H101)

            '! File transfer on the LabVIEW Real-Time (RT) target failed, because the
            '! required files could not be accessed. Solution: You may have executed a VI
            '! that opened the database, but did not close. If that is the case, you should
            '! change the VI to call Database Close, then reboot the RT controller to
            '! continue.
            Public Const nxErrFileTransferAccess As Integer = (NX_ERROR_BASE Or &H102)
            Public Const nxErrFTPFileAccess As Integer = (NX_ERROR_BASE Or &H102)

            '! The database file you want to use is already assigned to another alias.
            '! Solution: Each database file can only be assigned to a single alias. Use the
            '! alias that is already assigned to the database instead.
            Public Const nxErrDatabaseAlreadyInUse As Integer = (NX_ERROR_BASE Or &H103)

            '! An internal file used by NI-XNET could not be accessed. Solution: Make sure
            '! that the internal NI-XNET files are not write protected and that the
            '! directories for these files exist.
            Public Const nxErrInternalFileAccess As Integer = (NX_ERROR_BASE Or &H104)

            '! The file cannot be deployed because another file deployment is already
            '! active. Solution: wait until the other file deployment has finished and try
            '! again.
            Public Const nxErrFileTransferActive As Integer = (NX_ERROR_BASE Or &H105)

            '! The nixnet.dll or one of its components could not be loaded. Solution: try
            '! reinstalling NI-XNET. If the error persists,contact National Instruments.
            Public Const nxErrDllLoad As Integer = (NX_ERROR_BASE Or &H117)

            '! You attempted to perform an action on a session or interface that is
            '! started, and the action that requires the session/interface to be stopped.
            '! Solution: Stop the object before performing this action.
            Public Const nxErrObjectStarted As Integer = (NX_ERROR_BASE Or &H11E)

            '! You have passed a default payload to the firmware where the number of bytes
            '! in the payload is larger than the number of bytes that this frame can
            '! transmit. Solution: Decrease the number of bytes in your default payload.
            Public Const nxErrDefaultPayloadNumBytes As Integer = (NX_ERROR_BASE Or &H11F)

            '! You attempted to set a CAN arbitration ID with an invalid value. For
            '! example, a CAN standard arbitration ID supports only 11 bits. If you attempt
            '! to set a standard arbitration ID that uses more than 11 bits, this error is
            '! returned. Solution: Use a valid arbitration ID.
            Public Const nxErrInvalidArbitrationId As Integer = (NX_ERROR_BASE Or &H123)

            '! You attempted to set a LIN ID with an invalid value. For example, a LIN ID
            '! supports only 6 bits. If you attempt to set an ID that uses more than 6
            '! bits, this error is returned. Solution: Use a valid LIN ID.
            Public Const nxErrInvalidLinId As Integer = (NX_ERROR_BASE Or &H124)

            '! Too many open files. NI-XNET allows up to 7 database files to be opened
            '! simultaneously. Solution: Open fewer files.
            Public Const nxErrTooManyOpenFiles As Integer = (NX_ERROR_BASE Or &H130)

            '! Bad reference has been passed to a database function, e.g. a session
            '! reference, or frame reference to retrieve properties from a signal.
            Public Const nxErrDatabaseBadReference As Integer = (NX_ERROR_BASE Or &H131)

            '! Creating a database file failed. Solution: Verify access rights to the
            '! destination directory or check if overwritten file has read only permission.
            Public Const nxErrCreateDatabaseFile As Integer = (NX_ERROR_BASE Or &H132)

            '! A cluster with the same name already exists in the database. Solution: Use
            '! another name for this cluster.
            Public Const nxErrDuplicateClusterName As Integer = (NX_ERROR_BASE Or &H133)

            '! A frame with the same name already exists in the cluster. Solution: Use
            '! another name for this frame.
            Public Const nxErrDuplicateFrameName As Integer = (NX_ERROR_BASE Or &H134)

            '! A signal with the same name already exists in the frame. Solution: Use
            '! another name for this signal.
            Public Const nxErrDuplicateSignalName As Integer = (NX_ERROR_BASE Or &H135)

            '! An ECU with the same name already exists in the cluster. Solution: Use
            '! another name for this ECU.
            Public Const nxErrDuplicateECUName As Integer = (NX_ERROR_BASE Or &H136)

            '! A subframe with the same name already exists in the frame. Solution: Use
            '! another name for this subframe.
            Public Const nxErrDuplicateSubframeName As Integer = (NX_ERROR_BASE Or &H137)

            '! The operation is improper for the protocol in use, e.g. you cannot assign
            '! FlexRay channels to a CAN frame.
            Public Const nxErrImproperProtocol As Integer = (NX_ERROR_BASE Or &H138)

            '! Wrong parent relationship for a child that you are creating with XNET
            '! Database Create.
            Public Const nxErrObjectRelation As Integer = (NX_ERROR_BASE Or &H139)

            '! The retrieved required property is not defined on the specified object.
            '! Solution: Make sure that your database file has this property defined or
            '! that you set it in the objects created in memory.
            Public Const nxErrUnconfiguredRequiredProperty As Integer = (NX_ERROR_BASE Or &H13B)

            '! The feature is not supported under LabVIEW RT, e.g.Save Database
            Public Const nxErrNotSupportedOnRT As Integer = (NX_ERROR_BASE Or &H13C)

            '! The object name contains unsupported characters. The name must contain just
            '! alphanumeric characters and the underscore, but cannot begin with a digit.
            '! The maximum size is 128.
            Public Const nxErrNameSyntax As Integer = (NX_ERROR_BASE Or &H13D)

            '! Unsupported database format. For reading a database, the extension must be
            '! .xml, .dbc, .ncd, or .ldf. For saving, the extension must be .xml.
            Public Const nxErrFileExtension As Integer = (NX_ERROR_BASE Or &H13E)

            '! Database object not found, e.g. an object with given name doesn't exist.
            Public Const nxErrDatabaseObjectNotFound As Integer = (NX_ERROR_BASE Or &H13F)

            '! Database cache file cannot be removed or replaced on the disc, e.g. it is
            '! write-protected.
            Public Const nxErrRemoveDatabaseCacheFile As Integer = (NX_ERROR_BASE Or &H140)

            '! You are trying to write a read-only property, e.g. the mux value on a signal
            '! is a read only property (can be changed on the subframe).
            Public Const nxErrReadOnlyProperty As Integer = (NX_ERROR_BASE Or &H141)

            '! You are trying to change a signal to be a mux signal, but a mux is already
            '! defined in this frame
            Public Const nxErrFrameMuxExists As Integer = (NX_ERROR_BASE Or &H142)

            '! You are trying to define FlexRay in-cycle-repetition slots before defining
            '! the first slot. Define the first slot (frame ID) before defining
            '! in-cycle-repetition slots.
            Public Const nxErrUndefinedFirstSlot As Integer = (NX_ERROR_BASE Or &H144)

            '! You are trying to define FlexRay in-cycle-repetition channels before
            '! defining the first channels. Define the Channel Assignment on a frame before
            '! defining in-cycle-repetition channels.
            Public Const nxErrUndefinedFirstChannels As Integer = (NX_ERROR_BASE Or &H145)

            '! You must define the protocol before setting this property, e.g. the frame ID
            '! has a different meaning in a CAN or FlexRay cluster.
            Public Const nxErrUndefinedProtocol As Integer = (NX_ERROR_BASE Or &H146)

            '! The database information on the real-time system has been created with an
            '! older NI-XNET version. This version is no longer supported. To correct this
            '! error, re-deploy your database to the real-time system.
            Public Const nxErrOldDatabaseCacheFile As Integer = (NX_ERROR_BASE Or &H147)

            '! Frame ConfigStatus: A signal within the frame exceeds the frame boundaries
            '! (Payload Length).
            Public Const nxErrDbConfigSigOutOfFrame As Integer = (NX_ERROR_BASE Or &H148)

            '! Frame ConfigStatus: A signal within the frame overlaps another signal.
            Public Const nxErrDbConfigSigOverlapped As Integer = (NX_ERROR_BASE Or &H149)

            '! Frame ConfigStatus: A integer signal within the frame is defined with more
            '! than 52 bits. Not supported.
            Public Const nxErrDbConfigSig52BitInteger As Integer = (NX_ERROR_BASE Or &H14A)

            '! Frame ConfigStatus: Frame is defined with wrong number of bytes Allowed
            '! values: - CAN: 0-8, - Flexray: 0-254 and even number.
            Public Const nxErrDbConfigFrameNumBytes As Integer = (NX_ERROR_BASE Or &H14B)

            '! You are trying to add transmitted FlexRay frames to an ECU, with at least
            '! two of them having Startup or Sync property on. Only one Sync or Startup
            '! frame is allowed to be sent by an ECU.
            Public Const nxErrMultSyncStartup As Integer = (NX_ERROR_BASE Or &H14C)

            '! You are trying to add TX/RX frames to an ECU which are defined in a
            '! different cluster than the ECU.
            Public Const nxErrInvalidCluster As Integer = (NX_ERROR_BASE Or &H14D)

            '! Database name parameter is incorrect. Solution: Use a valid name for the
            '! database, e.g. ":memory:" for in-memory database.
            Public Const nxErrDatabaseName As Integer = (NX_ERROR_BASE Or &H14E)

            '! Database object is locked because it is used in a session. Solution:
            '! Configure the database before using it in a session.
            Public Const nxErrDatabaseObjectLocked As Integer = (NX_ERROR_BASE Or &H14F)

            '! Alias name passed to a function is not defined. Solution: Define the alias
            '! before calling the function.
            Public Const nxErrAliasNotFound As Integer = (NX_ERROR_BASE Or &H150)

            '! Database file cannot be saved because frames are assigned to FlexRay
            '! channels not defined in the cluster. Solution: Verify that all frames in the
            '! FlexRay cluster are assigned to an existing cluster channel.
            Public Const nxErrClusterFrameChannelRelation As Integer = (NX_ERROR_BASE Or &H151)

            '! Frame ConfigStatus: This FlexRay frame transmitted in a dynamic segment uses
            '! both channels A and B. This is not allowed. Solution: Use either channel A
            '! or B.
            Public Const nxErrDynFlexRayFrameChanAandB As Integer = (NX_ERROR_BASE Or &H152)

            '! Database is locked because it is being modified by an another instance of
            '! the same application. Solution: Close the database in the other application
            '! instance.
            Public Const nxErrDatabaseLockedInUse As Integer = (NX_ERROR_BASE Or &H153)

            '! A frame name is ambiguous, e.g. a frame with the same name exists in another
            '! cluster. Solution: Specify the cluster name for the frame using the required
            '! syntax.
            Public Const nxErrAmbiguousFrameName As Integer = (NX_ERROR_BASE Or &H154)

            '! A signal name is ambiguous, e.g. a signal with the same name exists in
            '! another frame. Solution: Use [frame].[signal] syntax for the signal.
            Public Const nxErrAmbiguousSignalName As Integer = (NX_ERROR_BASE Or &H155)

            '! An ECU name is ambiguous, e.g. an ECU with the same name exists in another
            '! cluster. Solution: Specify the cluster name for the ECU using the required
            '! syntax.
            Public Const nxErrAmbiguousECUName As Integer = (NX_ERROR_BASE Or &H156)

            '! A subframe name is ambiguous, e.g. a subframe with the same name exists in
            '! another cluster. Solution: Specify the cluster name for the subframe using
            '! the required syntax.
            Public Const nxErrAmbiguousSubframeName As Integer = (NX_ERROR_BASE Or &H157)

            '! A LIN schedule name is ambiguous, e.g. a schedule with the same name exists
            '! in another cluster. Solution: Specify the cluster name for the schedule
            '! using the required syntax.
            Public Const nxErrAmbiguousScheduleName As Integer = (NX_ERROR_BASE Or &H158)

            '! A LIN schedule with the same name already exists in the database. Solution:
            '! Use another name for this schedule.
            Public Const nxErrDuplicateScheduleName As Integer = (NX_ERROR_BASE Or &H159)

            '! A LIN diagnostic schedule change requires the diagnostic schedule to be
            '! defined in the database. Solution: Define the diagnostic schedule in the
            '! database.
            Public Const nxErrDiagnosticScheduleNotDefined As Integer = (NX_ERROR_BASE Or &H18F)

            '! Multiplexers (mode-dependent signals) are not supported when the given
            '! protocol is used. Solution: Contact National Instruments to see whether
            '! there is a newer NI-XNET version that supports multiplexers for the given
            '! protocol.
            Public Const nxErrProtocolMuxNotSupported As Integer = (NX_ERROR_BASE Or &H15A)

            '! Saving a FIBEX file containing a LIN cluster is not supported in this
            '! NI-XNET version. Solution: Contact National Instruments to see whether there
            '! is a newer NI-XNET version that supports saving a FIBEX file that contains a
            '! LIN cluster.
            Public Const nxErrSaveLINnotSupported As Integer = (NX_ERROR_BASE Or &H15B)

            '! This property requires an ECU configured as LIN master to be present in this
            '! cluster. Solution: Create a LIN master ECU in this cluster.
            Public Const nxErrLINmasterNotDefined As Integer = (NX_ERROR_BASE Or &H15C)

            '! You cannot mix open of NI-XNET database objects as both manual and
            '! automatic. You open manually by calling the Database Open VI. You open
            '! automatically when you 1) wire the IO name directly to a property node or
            '! VI, 2) branch a wire to multiple data flows on the diagram, 3) use the IO
            '! name with a VI or property node after closing it with the Database Close VI.
            '! Solution: Change your diagram to use the manual technique in all locations
            '! (always call Open and Close VIs), or to use the automatic technique in all
            '! locations (never call Open or Close VIs).
            Public Const nxErrMixAutoManualOpen As Integer = (NX_ERROR_BASE Or &H15E)

            '! Due to problems in LabVIEW versions 8.5 through 8.6.1, automatic open of
            '! NI-XNET database objects is not suppported. You open automatically when you
            '! 1) wire the IO name directly to a property node or VI, 2) branch a wire to
            '! multiple data flows on the diagram, 3) use the IO name with a VI or property
            '! node after closing it with the Database Close VI. Solution: Change your
            '! diagram to call the Database Open VI prior to any use (VI or property node)
            '! in a data flow (including a new wire branch). Change your diagram to call
            '! the Database Close VI when you are finished using the database in your
            '! application.
            Public Const nxErrAutoOpenNotSupported As Integer = (NX_ERROR_BASE Or &H15F)

            '! You called a Write function with the number of array elements (frames or
            '! signals) different than the number of elements configured in the session
            '! (such as the "list" parameter of the Create Session function). Solution:
            '! Write the same number of elements as configured in the session.
            Public Const nxErrWrongNumSignalsWritten As Integer = (NX_ERROR_BASE Or &H160)

            '! You used XNET session from multiple LabVIEW projects (or multiple
            '! executables), which NI-XNET does not support. Solution: Run XNET sessions in
            '! only one LabVIEW project at a time.
            Public Const nxErrMultipleLvProject As Integer = (NX_ERROR_BASE Or &H161)

            '! When an XNET session is used at runtime, all sessions in the same scope are
            '! created on the interface. The same scope is defined as all sessions within
            '! the same LabVIEW project which use the same cluster and interface (same
            '! physical cable configuration). If you attempt to use a session in the same
            '! scope after running the VI, this error occurs. The most likely cause is that
            '! you added a new session, and tried to use that new session in a running VI.
            '! Solution: Configure all session in LabVIEW project, then run the VI(s) that
            '! use those sessions.
            Public Const nxErrSessionConflictLvProject As Integer = (NX_ERROR_BASE Or &H162)

            '! You used an empty name for an XNET database object (database, cluster, ECU,
            '! frame, or signal). Empty name is not supported. Solution: Refer to NI-XNET
            '! help for IO names to review the required syntax for the name, and change
            '! your code to use that syntax.
            Public Const nxErrDbObjectNameEmpty As Integer = (NX_ERROR_BASE Or &H163)

            '! You used a name for an XNET database object (such as frame or signal) that
            '! did not include a valid cluster selection. Solution: Refer to the NI-XNET
            '! help for the IO name that you are using, and use the syntax specified for
            '! that class, which includes the cluster selection.
            Public Const nxErrMissingAliasInDbObjectName As Integer = (NX_ERROR_BASE Or &H164)

            '! Unsupported FIBEX file version. Solution: Use only FIBEX versions that are
            '! supported by this version of NI-XNET. Please see the NI-XNET documentation
            '! for information on which FIBEX versions are currently supported.
            Public Const nxErrFibexImportVersion As Integer = (NX_ERROR_BASE Or &H165)

            '! You used an empty name for the XNET Session. Empty name is not supported.
            '! Solution: Use a valid XNET session name from your LabVIEW project.
            Public Const nxErrEmptySessionName As Integer = (NX_ERROR_BASE Or &H166)

            '! There is not enough message RAM on the FlexRay hardware to configure the
            '! data partition for the object(s). Solution: Please refer to the manual for
            '! limitations on the number of objects that can be created at any given time
            '! based on the payload length.
            Public Const nxErrNotEnoughMessageRAMForObject As Integer = (NX_ERROR_BASE Or &H167)

            '! The FlexRay keyslot ID has been configured and a startup session has been
            '! created. Either the keyslot ID needs to be configured OR the startup session
            '! needs to be created. Both cannot exist at the same time. Solution: Choose a
            '! single method to configure startup sessions in your application.
            Public Const nxErrKeySlotIDConfig As Integer = (NX_ERROR_BASE Or &H168)

            '! An unsupported session was created. For example, stream output is not
            '! supported on FlexRay hardware. Solution: Only use supported sessions in your
            '! application.
            Public Const nxErrUnsupportedSession As Integer = (NX_ERROR_BASE Or &H169)


            '! An XNET session was created after starting the Interface. Only the Stream
            '! Input session in the subordinate mode can be created after the Interface has
            '! started. Solution: Create sessions prior to starting the XNET Interface in
            '! your application.
            Public Const nxErrObjectCreatedAfterStart As Integer = (NX_ERROR_BASE Or &H170)

            '! The Single Slot property was enabled on the XNET FlexRay Interface after the
            '! interface had started. Solution: Enable the Single Slot property prior to
            '! starting the XNET FlexRay Interface.
            Public Const nxErrSingleSlotEnabledAfterStart As Integer = (NX_ERROR_BASE Or &H171)

            '! The FlexRay macrotick offset specified for XNET Create Timing Source is
            '! unsupported. Example: Specifying a macrotick offset greater than
            '! MacroPerCycle will result in this error. Solution: Specify a macrotick
            '! offset within the supported range for the cluster.
            Public Const nxErrUnsupportedNumMacroticks As Integer = (NX_ERROR_BASE Or &H172)

            '! You used invalid syntax in the name of a database object (signal, frame, or
            '! ECU). For example, you may have specified a frame's name as
            '! [cluster].[frame], which is allowed in NI-XNET for C/C++, but not NI-XNET
            '! for LabVIEW. Solution: Use the string syntax specified in the help topic for
            '! the XNET I/O name class you are using.
            Public Const nxErrBadSyntaxInDatabaseObjectName As Integer = (NX_ERROR_BASE Or &H173)

            '! A LIN schedule entry name is ambiguous, e.g. a schedule entry with the same
            '! name exists in another schedule. Solution: Specify the schedule name for the
            '! schedule entry using the required syntax.
            Public Const nxErrAmbiguousScheduleEntryName As Integer = (NX_ERROR_BASE Or &H174)

            '! A LIN schedule entry with the same name already exists in the schedule.
            '! Solution: Use another name for this schedule entry.
            Public Const nxErrDuplicateScheduleEntryName As Integer = (NX_ERROR_BASE Or &H175)

            '! At least one of the frames in the session has an undefined identifier.
            '! Solution: Set the frame's "Identifier (Slot)" property before creating the
            '! session.
            Public Const nxErrUndefinedFrameId As Integer = (NX_ERROR_BASE Or &H176)

            '! At least one of the frames in the session has an undefined payload length.
            '! Solution: Set the frame's "Payload Length (in bytes)" property before
            '! creating the session.
            Public Const nxErrUndefinedFramePayloadLength As Integer = (NX_ERROR_BASE Or &H177)

            '! At least one of the signals in the session has an undefined start bit.
            '! Solution: Set the "Start Bit" property of the signal before creating the
            '! session.
            Public Const nxErrUndefinedSignalStartBit As Integer = (NX_ERROR_BASE Or &H178)

            '! At least one of the signals in the session has an undefined number of bits.
            '! Solution: Set the "Number of Bits" property of the signal before creating
            '! the session.
            Public Const nxErrUndefinedSignalNumBits As Integer = (NX_ERROR_BASE Or &H179)

            '! At least one of the signals in the session has an undefined byte order.
            '! Solution: Set the "Byte Order" property of the signal before creating the
            '! session.
            Public Const nxErrUndefinedSignalByteOrder As Integer = (NX_ERROR_BASE Or &H17A)

            '! At least one of the signals in the session has an undefined data type.
            '! Solution: Set the "Data Type" property of the signal before creating the
            '! session.
            Public Const nxErrUndefinedSignalDataType As Integer = (NX_ERROR_BASE Or &H17B)

            '! At least one of the subframes in the session has an undefined multiplexer
            '! value. Solution: Set the "Multiplexer Value" property of the subframe before
            '! creating the session.
            Public Const nxErrUndefinedSubfMuxValue As Integer = (NX_ERROR_BASE Or &H17C)

            '! You provided an invalid index to Write (State LIN Schedule Change).
            '! Solution: Use a number from to N-1, where N is the number of LIN schedules
            '! returned from the cluster property LIN Schedules. If you are using LabVIEW,
            '! the string for the number must be decimal (not hexadecimal).
            Public Const nxErrInvalidLinSchedIndex As Integer = (NX_ERROR_BASE Or &H17D)

            '! You provided an invalid name to Write (State LIN Schedule Change). Solution:
            '! Use a valid LIN schedule name returned from the cluster property LIN
            '! Schedules, or the session property Interface LIN Schedules. You can use the
            '! short name (schedule only) or long name (schedule plus database and
            '! cluster).
            Public Const nxErrInvalidLinSchedName As Integer = (NX_ERROR_BASE Or &H17E)

            '! You provided an invalid active index for the session property.
            Public Const nxErrInvalidActiveFrameIndex As Integer = (NX_ERROR_BASE Or &H17F)

            '! You provided an invalid name for Frame:Active of the session property node.
            '! Solution: Use a valid item name from the session's List property. You can
            '! use the short name (frame or signal only) or long name (frame/signal plus
            '! database and cluster).
            Public Const nxErrInvalidActiveFrameName As Integer = (NX_ERROR_BASE Or &H180)

            '! The database you are using requires using PDUs, and the operation is
            '! ambiguous with respect to PDUs. Example: You are trying to get the frame
            '! parent of the signal, but the PDU in which the signal is contained is
            '! referenced in multiple frames.
            Public Const nxErrAmbiguousPDU As Integer = (NX_ERROR_BASE Or &H181)

            '! A PDU with the same name already exists in the cluster. Solution: Use
            '! another name for this PDU.
            Public Const nxErrDuplicatePDU As Integer = (NX_ERROR_BASE Or &H182)

            '! You are trying to assign start bits or update bits to PDUs referenced in a
            '! frame, but the number of elements in this array is different than the number
            '! of referenced PDUs. Solution: Use the same number of elements in the array
            '! as in the PDU references array.
            Public Const nxErrNumberOfPDUs As Integer = (NX_ERROR_BASE Or &H183)

            '! The configuration of this object requires using advanced PDUs, which the
            '! given protocol does not support. Solution: You cannot use this object in the
            '! given protocol.
            Public Const nxErrPDUsRequired As Integer = (NX_ERROR_BASE Or &H184)

            '! The maximum number of PDUs has been exceeded. Solution: Use fewer PDUs in
            '! your sessions.
            Public Const nxErrMaxPDUs As Integer = (NX_ERROR_BASE Or &H185)

            '! This mode value is not currently supported. Solution: Use a valid value.
            Public Const nxErrUnsupportedMode As Integer = (NX_ERROR_BASE Or &H186)

            '! The firmware image on your XNET C Series module is corrupted. Solution:
            '! Update the firmware of this C Series module in MAX.
            Public Const nxErrBadcSeriesFpgaSignature As Integer = (NX_ERROR_BASE Or &H187)

            '! The firmware version of your XNET C Series module is not in sync with your
            '! host computer. Solution: Update the firmware of this C Series module in MAX.
            Public Const nxErrBadcSeriesFpgaRevision As Integer = (NX_ERROR_BASE Or &H188)

            '! The firmware version of your XNET C Series module is not in sync with the
            '! NI-XNET software on your remote target. Solution: Update the NI-XNETsoftware
            '! on the remote target.
            Public Const nxErrBadFpgaRevisionOnTarget As Integer = (NX_ERROR_BASE Or &H189)

            '! The terminal you are trying to use is already in use. Only one connection
            '! per terminal is allowed. Solution: disconnect the terminal that is already
            '! in use.
            Public Const nxErrRouteInUse As Integer = (NX_ERROR_BASE Or &H18A)

            '! You need to install a supported version of NI-DAQmx for your XNET C Series
            '! module to work correctly with your Compact DAQ system. Solution: Check the
            '! NI-XNET readme file for supported versions of the NI-DAQmx driver software.
            Public Const nxErrDAQmxIncorrectVersion As Integer = (NX_ERROR_BASE Or &H18B)

            '! The NI-DAQmx driver cannot create the requested route. Solution: Resolve
            '! routing conflicts or invalid terminal names.
            Public Const nxErrAddRoute As Integer = (NX_ERROR_BASE Or &H18C)

            '! You attempted to transmit a go to sleep frame (by setting the LIN Sleep
            '! mode to Remote Sleep) on a LIN interface configured as slave.  In
            '! conformance with the LIN protocol standard, only an interface configured
            '! as master may transmit a go to sleep frame.
            Public Const nxErrRemoteSleepOnLinSlave As Integer = (NX_ERROR_BASE Or &H18D)

            '! You attempted to set properties related to Sleep and Wakeup
            '! when the FlexRay cluster defined in the Fibex file does not support it.
            '! Solution: Edit the Fibex file used in your application to include all
            '! relevant cluster wakeup attributes.
            Public Const nxErrSleepWakeupNotSupported As Integer = (NX_ERROR_BASE Or &H18E)

            '! The data payload written for a diagnostic frame for transmit does not
            '! conform to the LIN transport layer specification.  Solution: Ensure the
            '! data payload for a diagnostic frame conforms to the transport layer
            '! specification.
            Public Const nxErrLINTransportLayer As Integer = (NX_ERROR_BASE Or &H192)

            '! An error occurred within the NI-XNET example code for logfile access (TDMS).
            '! Solution: For LabVIEW, the subVI with the error is shown as the source,
            '! and you can open that subVI to determine the cause of the problem.
            '! For other programming languages, review the source code for the logfile
            '! example to determine the cause of the problem.
            Public Const nxErrLogfile As Integer = (NX_ERROR_BASE Or &H193)

            ' Warning Section

            '! There is a warning from importing the database file. For details, refer to
            '! the import log file nixnetfx-log.txt or nixnetldf-log.txt under
            '! %ALLUSERSPROFILE%\\Application Data\\National Instruments\\NI-XNET. Please note
            '! that this location may be hidden on your computer.
            Public Const nxWarnDatabaseImport As Integer = (NX_WARNING_BASE Or &H85)

            '! The database file has been imported, but it was not created by the XNET
            '! Editor or using the XNET API. Saving the database file with the XNET API or
            '! XNET Editor may lose information from the original file.
            Public Const nxWarnDatabaseImportFIBEXNoXNETFile As Integer = (NX_WARNING_BASE Or &H86)

            '! The database file was not created by the XNET Editor or using the XNET API.
            '! Additionally, there is another warning. For details, refer to the import log
            '! file nixnetfx-log.txt under %ALLUSERSPROFILE%\\Application Data\\National
            '! Instruments\\NI-XNET.
            Public Const nxWarnDatabaseImportFIBEXNoXNETFilePlusWarning As Integer = (NX_WARNING_BASE Or &H87)

            '! Close Database returns a warning instead of an error when an invalid
            '! reference is passed to the function.
            Public Const nxWarnDatabaseBadReference As Integer = (NX_WARNING_BASE Or &H131)

            '! Your are retrieving signals from a frame that uses advanced PDU
            '! configuration. The signal start bit is given relative to the PDU, and it may
            '! be different than the start bit relative to the frame.
            Public Const nxWarnAdvancedPDU As Integer = (NX_WARNING_BASE Or &H132)

            '! The multiplexer size exceeds 16 bit. This is not supported for Single Point
            '! sessions.
            Public Const nxWarnMuxExceeds16Bit As Integer = (NX_WARNING_BASE Or &H133)


            '**********************************************************************
            '                                      P R O P E R T Y   I D S
            '            **********************************************************************


            ' Class IDs used for encoding of property IDs (nxProp*)
            ' Also class parameter of function nxdbCreateObject, nxdbDeleteObject, and nxdbFindObject
            Public Const nxClass_Database As UInteger = &H0
            'Database
            Public Const nxClass_Cluster As UInteger = &H10000
            'Cluster
            Public Const nxClass_Frame As UInteger = &H20000
            'Frame
            Public Const nxClass_Signal As UInteger = &H30000
            'Signal
            Public Const nxClass_Subframe As UInteger = &H40000
            'Subframe
            Public Const nxClass_ECU As UInteger = &H50000
            'ECU
            Public Const nxClass_LINSched As UInteger = &H60000
            'LIN Schedule
            Public Const nxClass_LINSchedEntry As UInteger = &H70000
            'LIN Schedule Entry
            Public Const nxClass_PDU As UInteger = &H80000
            'PDU
            Public Const nxClass_Session As UInteger = &H100000
            'Session
            Public Const nxClass_System As UInteger = &H110000
            'System
            Public Const nxClass_Device As UInteger = &H120000
            'Device
            Public Const nxClass_Interface As UInteger = &H130000
            'Interface
            Public Const nxClass_Mask As UInteger = &HFF0000
            'mask for object class
            ' Datatype IDs used in encoding of property IDs (nxProp*)
            Public Const nxPrptype_uint As UInteger = &H0
            Public Const nxPrptype_f64 As UInteger = &H1000000
            Public Const nxPrptype_bool As UInteger = &H2000000
            ' use byte as datatype (semantic only)
            Public Const nxPrptype_string As UInteger = &H3000000
            Public Const nxPrptype_1Dstring As UInteger = &H4000000
            ' comma-separated list
            Public Const nxPrptype_ref As UInteger = &H5000000
            ' uint reference (handle)
            Public Const nxPrptype_1Dref As UInteger = &H6000000
            ' array of uint reference
            Public Const nxPrptype_time As UInteger = &H7000000
            ' nxTimestamp_t
            Public Const nxPrptype_1Duint As UInteger = &H8000000
            ' array of uint values
            Public Const nxPrptype_u64 As UInteger = &H9000000
            Public Const nxPrptype_1Dbyte As UInteger = &HA000000
            ' array of byte values
            Public Const nxPrptype_Mask As UInteger = &HFF000000UI
            ' mask for proptype
            ' PropertyId parameter of nxGetProperty, nxGetPropertySize, nxSetProperty functions. 

            ' Session:Auto Start?
            Public Const nxPropSession_AutoStart As UInteger = (CUInt(&H1) Or nxClass_Session Or nxPrptype_bool)
            '--rw
            ' Session:ClusterName
            Public Const nxPropSession_ClusterName As UInteger = (CUInt(&HA) Or nxClass_Session Or nxPrptype_string)
            '--r
            ' Session:Database
            Public Const nxPropSession_DatabaseName As UInteger = (CUInt(&H2) Or nxClass_Session Or nxPrptype_string)
            '--r
            ' Session:List of Signals / List of Frames
            Public Const nxPropSession_List As UInteger = (CUInt(&H3) Or nxClass_Session Or nxPrptype_1Dstring)
            '--r
            ' Session:Mode
            Public Const nxPropSession_Mode As UInteger = (CUInt(&H4) Or nxClass_Session Or nxPrptype_uint)
            '--r
            ' Session:Number of Frames
            Public Const nxPropSession_NumFrames As UInteger = (CUInt(&HD) Or nxClass_Session Or nxPrptype_uint)
            '--r
            ' Session:Number in List
            Public Const nxPropSession_NumInList As UInteger = (CUInt(&H5) Or nxClass_Session Or nxPrptype_uint)
            '--r
            ' Session:Number of Values Pending
            Public Const nxPropSession_NumPend As UInteger = (CUInt(&H6) Or nxClass_Session Or nxPrptype_uint)
            '--r
            ' Session:Number of Values Unused
            Public Const nxPropSession_NumUnused As UInteger = (CUInt(&HB) Or nxClass_Session Or nxPrptype_uint)
            '--r
            ' Session:Payload Length Maximum
            Public Const nxPropSession_PayldLenMax As UInteger = (CUInt(&H9) Or nxClass_Session Or nxPrptype_uint)
            '--r
            ' Session:Protocol
            Public Const nxPropSession_Protocol As UInteger = (CUInt(&H8) Or nxClass_Session Or nxPrptype_uint)
            '--r
            ' Session:Queue Size
            Public Const nxPropSession_QueueSize As UInteger = (CUInt(&HC) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Resample Rate
            Public Const nxPropSession_ResampRate As UInteger = (CUInt(&H7) Or nxClass_Session Or nxPrptype_f64)
            '--rw
            ' Session:Interface:Baud Rate
            Public Const nxPropSession_IntfBaudRate As UInteger = (CUInt(&H16) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:Bus Error Frames to Input Stream?
            Public Const nxPropSession_IntfBusErrToInStrm As UInteger = (CUInt(&H15) Or nxClass_Session Or nxPrptype_bool)
            '--rw
            ' Session:Interface:Echo Transmit?
            Public Const nxPropSession_IntfEchoTx As UInteger = (CUInt(&H10) Or nxClass_Session Or nxPrptype_bool)
            '--rw
            ' Session:Interface:Name
            Public Const nxPropSession_IntfName As UInteger = (CUInt(&H13) Or nxClass_Session Or nxPrptype_string)
            '--r
            ' Session:Interface:Output Stream List
            Public Const nxPropSession_IntfOutStrmList As UInteger = (CUInt(&H11) Or nxClass_Session Or nxPrptype_1Dref)
            '--rw
            ' Session:Interface:Output Stream Timing
            Public Const nxPropSession_IntfOutStrmTimng As UInteger = (CUInt(&H12) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:Start Trigger Frames to Input Stream?
            Public Const nxPropSession_IntfStartTrigToInStrm As UInteger = (CUInt(&H14) Or nxClass_Session Or nxPrptype_bool)
            '--rw
            ' Session:Interface:CAN:External Transceiver Configuration
            Public Const nxPropSession_IntfCANExtTcvrConfig As UInteger = (CUInt(&H23) Or nxClass_Session Or nxPrptype_uint)
            '--w
            ' Session:Interface:CAN:Listen Only?
            Public Const nxPropSession_IntfCANLstnOnly As UInteger = (CUInt(&H22) Or nxClass_Session Or nxPrptype_bool)
            '--rw
            ' Session:Interface:CAN:Pending Transmit Order
            Public Const nxPropSession_IntfCANPendTxOrder As UInteger = (CUInt(&H20) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:CAN:Single Shot Transmit?
            Public Const nxPropSession_IntfCANSingShot As UInteger = (CUInt(&H24) Or nxClass_Session Or nxPrptype_bool)
            '--rw
            ' Session:Interface:CAN:Termination
            Public Const nxPropSession_IntfCANTerm As UInteger = (CUInt(&H25) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:CAN:Transceiver State
            Public Const nxPropSession_IntfCANTcvrState As UInteger = (CUInt(&H28) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:CAN:Transceiver Type
            Public Const nxPropSession_IntfCANTcvrType As UInteger = (CUInt(&H29) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:CAN:Output Stream List By ID
            Public Const nxPropSession_IntfCANOutStrmListById As UInteger = (CUInt(&H21) Or nxClass_Session Or nxPrptype_1Duint)
            '--rw
            ' Session:Interface:FlexRay:Accepted Startup Range
            Public Const nxPropSession_IntfFlexRayAccStartRng As UInteger = (CUInt(&H30) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Allow Halt Due To Clock?
            Public Const nxPropSession_IntfFlexRayAlwHltClk As UInteger = (CUInt(&H31) Or nxClass_Session Or nxPrptype_bool)
            '--rw
            ' Session:Interface:FlexRay:Allow Passive to Active
            Public Const nxPropSession_IntfFlexRayAlwPassAct As UInteger = (CUInt(&H32) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Auto Asleep When Stopped
            Public Const nxPropSession_IntfFlexRayAutoAslpWhnStp As UInteger = (CUInt(&H3A) Or nxClass_Session Or nxPrptype_bool)
            '--rw
            ' Session:Interface:FlexRay:Cluster Drift Damping
            Public Const nxPropSession_IntfFlexRayClstDriftDmp As UInteger = (CUInt(&H33) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Coldstart?
            Public Const nxPropSession_IntfFlexRayColdstart As UInteger = (CUInt(&H34) Or nxClass_Session Or nxPrptype_bool)
            '--r
            ' Session:Interface:FlexRay:Decoding Correction
            Public Const nxPropSession_IntfFlexRayDecCorr As UInteger = (CUInt(&H35) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Delay Compensation Ch A
            Public Const nxPropSession_IntfFlexRayDelayCompA As UInteger = (CUInt(&H36) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Delay Compensation Ch B
            Public Const nxPropSession_IntfFlexRayDelayCompB As UInteger = (CUInt(&H37) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Key Slot Identifier
            Public Const nxPropSession_IntfFlexRayKeySlotID As UInteger = (CUInt(&H38) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Latest Tx
            Public Const nxPropSession_IntfFlexRayLatestTx As UInteger = (CUInt(&H41) Or nxClass_Session Or nxPrptype_uint)
            '--r
            ' Session:Interface:FlexRay:Listen Timeout
            Public Const nxPropSession_IntfFlexRayListTimo As UInteger = (CUInt(&H42) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Macro Initial Offset Ch A
            Public Const nxPropSession_IntfFlexRayMacInitOffA As UInteger = (CUInt(&H43) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Macro Initial Offset Ch B
            Public Const nxPropSession_IntfFlexRayMacInitOffB As UInteger = (CUInt(&H44) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Micro Initial Offset Ch A
            Public Const nxPropSession_IntfFlexRayMicInitOffA As UInteger = (CUInt(&H45) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Micro Initial Offset Ch B
            Public Const nxPropSession_IntfFlexRayMicInitOffB As UInteger = (CUInt(&H46) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Max Drift
            Public Const nxPropSession_IntfFlexRayMaxDrift As UInteger = (CUInt(&H47) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Microtick
            Public Const nxPropSession_IntfFlexRayMicrotick As UInteger = (CUInt(&H48) Or nxClass_Session Or nxPrptype_uint)
            '--r
            ' Session:Interface:FlexRay:Null Frames To Input Stream?
            Public Const nxPropSession_IntfFlexRayNullToInStrm As UInteger = (CUInt(&H49) Or nxClass_Session Or nxPrptype_bool)
            '--rw
            ' Session:Interface:FlexRay:Offset Correction
            Public Const nxPropSession_IntfFlexRayOffCorr As UInteger = (CUInt(&H58) Or nxClass_Session Or nxPrptype_uint)
            '--r
            ' Session:Interface:FlexRay:Offset Correction Out
            Public Const nxPropSession_IntfFlexRayOffCorrOut As UInteger = (CUInt(&H50) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Rate Correction
            Public Const nxPropSession_IntfFlexRayRateCorr As UInteger = (CUInt(&H59) Or nxClass_Session Or nxPrptype_uint)
            '--r
            ' Session:Interface:FlexRay:Rate Correction Out
            Public Const nxPropSession_IntfFlexRayRateCorrOut As UInteger = (CUInt(&H52) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Samples Per Microtick
            Public Const nxPropSession_IntfFlexRaySampPerMicro As UInteger = (CUInt(&H53) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Single Slot Enabled?
            Public Const nxPropSession_IntfFlexRaySingSlotEn As UInteger = (CUInt(&H54) Or nxClass_Session Or nxPrptype_bool)
            '--rw
            ' Session:Interface:FlexRay:Statistics Enabled?
            Public Const nxPropSession_IntfFlexRayStatisticsEn As UInteger = (CUInt(&H5A) Or nxClass_Session Or nxPrptype_bool)
            '--rw
            ' Session:Interface:FlexRay:Symbol Frames To Input Stream?
            Public Const nxPropSession_IntfFlexRaySymToInStrm As UInteger = (CUInt(&H3D) Or nxClass_Session Or nxPrptype_bool)
            '--rw
            ' Session:Interface:FlexRay:Sync on Ch A Even Cycle
            Public Const nxPropSession_IntfFlexRaySyncChAEven As UInteger = (CUInt(&H5B) Or nxClass_Session Or nxPrptype_1Duint)
            '--r
            ' Session:Interface:FlexRay:Sync on Ch A Odd Cycle
            Public Const nxPropSession_IntfFlexRaySyncChAOdd As UInteger = (CUInt(&H5C) Or nxClass_Session Or nxPrptype_1Duint)
            '--r
            ' Session:Interface:FlexRay:Sync on Ch B Even Cycle
            Public Const nxPropSession_IntfFlexRaySyncChBEven As UInteger = (CUInt(&H5D) Or nxClass_Session Or nxPrptype_1Duint)
            '--r
            ' Session:Interface:FlexRay:Sync on Ch B Odd Cycle
            Public Const nxPropSession_IntfFlexRaySyncChBOdd As UInteger = (CUInt(&H5E) Or nxClass_Session Or nxPrptype_1Duint)
            '--r
            ' Session:Interface:FlexRay:Sync Frame Status
            Public Const nxPropSession_IntfFlexRaySyncStatus As UInteger = (CUInt(&H5F) Or nxClass_Session Or nxPrptype_uint)
            '--r
            ' Session:Interface:FlexRay:Termination
            Public Const nxPropSession_IntfFlexRayTerm As UInteger = (CUInt(&H57) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Wakeup Channel
            Public Const nxPropSession_IntfFlexRayWakeupCh As UInteger = (CUInt(&H55) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Wakeup Pattern
            Public Const nxPropSession_IntfFlexRayWakeupPtrn As UInteger = (CUInt(&H56) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:FlexRay:Sleep
            Public Const nxPropSession_IntfFlexRaySleep As UInteger = (CUInt(&H3B) Or nxClass_Session Or nxPrptype_uint)
            '--w
            ' Session:Interface:FlexRay:Connected Channels
            Public Const nxPropSession_IntfFlexRayConnectedChs As UInteger = (CUInt(&H3C) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:LIN:Break Length
            Public Const nxPropSession_IntfLINBreakLength As UInteger = (CUInt(&H70) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:LIN:Master?
            Public Const nxPropSession_IntfLINMaster As UInteger = (CUInt(&H72) Or nxClass_Session Or nxPrptype_bool)
            '--rw
            ' Session:Interface:LIN:Schedule Names
            Public Const nxPropSession_IntfLINSchedNames As UInteger = (CUInt(&H75) Or nxClass_Session Or nxPrptype_1Dstring)
            '--r
            ' Session:Interface:LIN:Sleep
            Public Const nxPropSession_IntfLINSleep As UInteger = (CUInt(&H73) Or nxClass_Session Or nxPrptype_uint)
            '--w
            ' Session:Interface:LIN:Termination
            Public Const nxPropSession_IntfLINTerm As UInteger = (CUInt(&H74) Or nxClass_Session Or nxPrptype_uint)
            '--rw
            ' Session:Interface:LIN:Diagnostics P2min
            Public Const nxPropSession_IntfLINDiagP2min As UInteger = (CUInt(&H77) Or nxClass_Session Or nxPrptype_f64)
            '--rw
            ' Session:Interface:LIN:Diagnostics STmin
            Public Const nxPropSession_IntfLINDiagSTmin As UInteger = (CUInt(&H76) Or nxClass_Session Or nxPrptype_f64)
            '--rw
            ' Session:Interface:Source Terminal:Start Trigger
            Public Const nxPropSession_IntfSrcTermStartTrigger As UInteger = (CUInt(&H90) Or nxClass_Session Or nxPrptype_string)
            '--rw
            ' System:Devices
            Public Const nxPropSys_DevRefs As UInteger = (CUInt(&H2) Or nxClass_System Or nxPrptype_1Dref)
            '--r
            ' System:Interfaces (All)
            Public Const nxPropSys_IntfRefs As UInteger = (CUInt(&H3) Or nxClass_System Or nxPrptype_1Dref)
            '--r
            ' System:Interfaces (CAN)
            Public Const nxPropSys_IntfRefsCAN As UInteger = (CUInt(&H4) Or nxClass_System Or nxPrptype_1Dref)
            '--r
            ' System:Interfaces (FlexRay)
            Public Const nxPropSys_IntfRefsFlexRay As UInteger = (CUInt(&H5) Or nxClass_System Or nxPrptype_1Dref)
            '--r
            ' System:Interfaces (LIN)
            Public Const nxPropSys_IntfRefsLIN As UInteger = (CUInt(&H7) Or nxClass_System Or nxPrptype_1Dref)
            '--r
            ' System:Version:Build
            Public Const nxPropSys_VerBuild As UInteger = (CUInt(&H6) Or nxClass_System Or nxPrptype_uint)
            '--r
            ' System:Version:Major
            Public Const nxPropSys_VerMajor As UInteger = (CUInt(&H8) Or nxClass_System Or nxPrptype_uint)
            '--r
            ' System:Version:Minor
            Public Const nxPropSys_VerMinor As UInteger = (CUInt(&H9) Or nxClass_System Or nxPrptype_uint)
            '--r
            ' System:Version:Phase
            Public Const nxPropSys_VerPhase As UInteger = (CUInt(&HA) Or nxClass_System Or nxPrptype_uint)
            '--r
            ' System:Version:Update
            Public Const nxPropSys_VerUpdate As UInteger = (CUInt(&HB) Or nxClass_System Or nxPrptype_uint)
            '--r
            ' System:CompactDAQ Packet Time
            Public Const nxPropSys_CDAQPktTime As UInteger = (CUInt(&HC) Or nxClass_System Or nxPrptype_f64)
            '--rw
            ' Device:Form Factor
            Public Const nxPropDev_FormFac As UInteger = (CUInt(&H1) Or nxClass_Device Or nxPrptype_uint)
            '--r
            ' Device:Interfaces
            Public Const nxPropDev_IntfRefs As UInteger = (CUInt(&H2) Or nxClass_Device Or nxPrptype_1Dref)
            '--r
            ' Device:Name
            Public Const nxPropDev_Name As UInteger = (CUInt(&H3) Or nxClass_Device Or nxPrptype_string)
            '--r
            ' Device:Number of Ports
            Public Const nxPropDev_NumPorts As UInteger = (CUInt(&H4) Or nxClass_Device Or nxPrptype_uint)
            '--r
            ' Device:Product Number
            Public Const nxPropDev_ProductNum As UInteger = (CUInt(&H8) Or nxClass_Device Or nxPrptype_uint)
            '--r
            ' Device:Serial Number
            Public Const nxPropDev_SerNum As UInteger = (CUInt(&H5) Or nxClass_Device Or nxPrptype_uint)
            '--r
            ' Device:Slot Number
            Public Const nxPropDev_SlotNum As UInteger = (CUInt(&H6) Or nxClass_Device Or nxPrptype_uint)
            '--r
            ' Interface:Device
            Public Const nxPropIntf_DevRef As UInteger = (CUInt(&H1) Or nxClass_Interface Or nxPrptype_ref)
            '--r
            ' Interface:Name
            Public Const nxPropIntf_Name As UInteger = (CUInt(&H2) Or nxClass_Interface Or nxPrptype_string)
            '--r
            ' Interface:Number
            Public Const nxPropIntf_Num As UInteger = (CUInt(&H3) Or nxClass_Interface Or nxPrptype_uint)
            '--r
            ' Interface:Port Number
            Public Const nxPropIntf_PortNum As UInteger = (CUInt(&H4) Or nxClass_Interface Or nxPrptype_uint)
            '--r
            ' Interface:Protocol
            Public Const nxPropIntf_Protocol As UInteger = (CUInt(&H5) Or nxClass_Interface Or nxPrptype_uint)
            '--r
            ' Interface:CAN:Termination Capability
            Public Const nxPropIntf_CANTermCap As UInteger = (CUInt(&H8) Or nxClass_Interface Or nxPrptype_uint)
            '--r
            ' Interface:CAN:Transceiver Capability
            Public Const nxPropIntf_CANTcvrCap As UInteger = (CUInt(&H7) Or nxClass_Interface Or nxPrptype_uint)
            '--r

            ' PropertyId parameter of nxGetSubProperty, nxGetSubPropertySize, nxSetSubProperty functions. 


            ' Session:Frame:CAN:Start Time Offset
            Public Const nxPropSessionSub_CANStartTimeOff As UInteger = (CUInt(&H81) Or nxClass_Session Or nxPrptype_f64)
            '--w
            ' Session:Frame:CAN:Transmit Time
            Public Const nxPropSessionSub_CANTxTime As UInteger = (CUInt(&H82) Or nxClass_Session Or nxPrptype_f64)
            '--w
            ' Session:Frame:CAN:Skip N Cyclic Frames
            Public Const nxPropSessionSub_SkipNCyclicFrames As UInteger = (CUInt(&H83) Or nxClass_Session Or nxPrptype_uint)
            '--w

            ' PropertyId parameter of nxdbGetProperty, nxdbGetPropertySize, nxdbSetProperty functions. 


            ' Database:Name
            Public Const nxPropDatabase_Name As UInteger = (CUInt(&H1) Or nxClass_Database Or nxPrptype_string)
            '--r
            ' Database:Clusters
            Public Const nxPropDatabase_ClstRefs As UInteger = (CUInt(&H2) Or nxClass_Database Or nxPrptype_1Dref)
            '--r
            ' Database:Show Invalid Frames and Signals
            Public Const nxPropDatabase_ShowInvalidFromOpen As UInteger = (CUInt(&H3) Or nxClass_Database Or nxPrptype_bool)
            '--rw
            ' Cluster:Baud Rate
            Public Const nxPropClst_BaudRate As UInteger = (CUInt(&H1) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:Comment
            Public Const nxPropClst_Comment As UInteger = (CUInt(&H8) Or nxClass_Cluster Or nxPrptype_string)
            '--rw
            ' Cluster:Configuration Status
            Public Const nxPropClst_ConfigStatus As UInteger = (CUInt(&H9) Or nxClass_Cluster Or nxPrptype_uint)
            '--r
            ' Cluster:Database
            Public Const nxPropClst_DatabaseRef As UInteger = (CUInt(&H2) Or nxClass_Cluster Or nxPrptype_ref)
            '--r
            ' Cluster:ECUs
            Public Const nxPropClst_ECURefs As UInteger = (CUInt(&H3) Or nxClass_Cluster Or nxPrptype_1Dref)
            '--r
            ' Cluster:Frames
            Public Const nxPropClst_FrmRefs As UInteger = (CUInt(&H4) Or nxClass_Cluster Or nxPrptype_1Dref)
            '--r
            ' Cluster:Name (Short)
            Public Const nxPropClst_Name As UInteger = (CUInt(&H5) Or nxClass_Cluster Or nxPrptype_string)
            '--rw
            ' Cluster:PDUs
            Public Const nxPropClst_PDURefs As UInteger = (CUInt(&H8) Or nxClass_Cluster Or nxPrptype_1Dref)
            '--r
            ' Cluster:PDUs Required?
            Public Const nxPropClst_PDUsReqd As UInteger = (CUInt(&HA) Or nxClass_Cluster Or nxPrptype_bool)
            '--r
            ' Cluster:Protocol
            Public Const nxPropClst_Protocol As UInteger = (CUInt(&H6) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:Signals
            Public Const nxPropClst_SigRefs As UInteger = (CUInt(&H7) Or nxClass_Cluster Or nxPrptype_1Dref)
            '--r
            ' Cluster:FlexRay:Action Point Offset
            Public Const nxPropClst_FlexRayActPtOff As UInteger = (CUInt(&H20) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:CAS Rx Low Max
            Public Const nxPropClst_FlexRayCASRxLMax As UInteger = (CUInt(&H21) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Channels
            Public Const nxPropClst_FlexRayChannels As UInteger = (CUInt(&H22) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Cluster Drift Damping
            Public Const nxPropClst_FlexRayClstDriftDmp As UInteger = (CUInt(&H23) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Cold Start Attempts
            Public Const nxPropClst_FlexRayColdStAts As UInteger = (CUInt(&H24) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Cycle
            Public Const nxPropClst_FlexRayCycle As UInteger = (CUInt(&H25) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Dynamic Segment Start
            Public Const nxPropClst_FlexRayDynSegStart As UInteger = (CUInt(&H26) Or nxClass_Cluster Or nxPrptype_uint)
            '--r
            ' Cluster:FlexRay:Dynamic Slot Idle Phase
            Public Const nxPropClst_FlexRayDynSlotIdlPh As UInteger = (CUInt(&H27) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Latest Usable Dynamic Slot
            Public Const nxPropClst_FlexRayLatestUsableDyn As UInteger = (CUInt(&H2A) Or nxClass_Cluster Or nxPrptype_uint)
            '--r
            ' Cluster:FlexRay:Latest Guaranteed Dynamic Slot
            Public Const nxPropClst_FlexRayLatestGuarDyn As UInteger = (CUInt(&H2B) Or nxClass_Cluster Or nxPrptype_uint)
            '--r
            ' Cluster:FlexRay:Listen Noise
            Public Const nxPropClst_FlexRayLisNoise As UInteger = (CUInt(&H28) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Macro Per Cycle
            Public Const nxPropClst_FlexRayMacroPerCycle As UInteger = (CUInt(&H29) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Macrotick
            Public Const nxPropClst_FlexRayMacrotick As UInteger = (CUInt(&H30) Or nxClass_Cluster Or nxPrptype_f64)
            '--r
            ' Cluster:FlexRay:Max Without Clock Correction Fatal
            Public Const nxPropClst_FlexRayMaxWoClkCorFat As UInteger = (CUInt(&H31) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Max Without Clock Correction Passive
            Public Const nxPropClst_FlexRayMaxWoClkCorPas As UInteger = (CUInt(&H32) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Minislot Action Point Offset
            Public Const nxPropClst_FlexRayMinislotActPt As UInteger = (CUInt(&H33) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Minislot
            Public Const nxPropClst_FlexRayMinislot As UInteger = (CUInt(&H34) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Network Management Vector Length
            Public Const nxPropClst_FlexRayNMVecLen As UInteger = (CUInt(&H35) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:NIT
            Public Const nxPropClst_FlexRayNIT As UInteger = (CUInt(&H36) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:NIT Start
            Public Const nxPropClst_FlexRayNITStart As UInteger = (CUInt(&H37) Or nxClass_Cluster Or nxPrptype_uint)
            '--r
            ' Cluster:FlexRay:Number Of Minislots
            Public Const nxPropClst_FlexRayNumMinislt As UInteger = (CUInt(&H38) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Number Of Static Slots
            Public Const nxPropClst_FlexRayNumStatSlt As UInteger = (CUInt(&H39) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Offset Correction Start
            Public Const nxPropClst_FlexRayOffCorSt As UInteger = (CUInt(&H40) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Payload Length Dynamic Maximum
            Public Const nxPropClst_FlexRayPayldLenDynMax As UInteger = (CUInt(&H41) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Payload Length Maximum
            Public Const nxPropClst_FlexRayPayldLenMax As UInteger = (CUInt(&H42) Or nxClass_Cluster Or nxPrptype_uint)
            '--r
            ' Cluster:FlexRay:Payload Length Static
            Public Const nxPropClst_FlexRayPayldLenSt As UInteger = (CUInt(&H43) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Static Slot
            Public Const nxPropClst_FlexRayStatSlot As UInteger = (CUInt(&H45) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Symbol Window
            Public Const nxPropClst_FlexRaySymWin As UInteger = (CUInt(&H46) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Symbol Window Start
            Public Const nxPropClst_FlexRaySymWinStart As UInteger = (CUInt(&H47) Or nxClass_Cluster Or nxPrptype_uint)
            '--r
            ' Cluster:FlexRay:Sync Node Max
            Public Const nxPropClst_FlexRaySyncNodeMax As UInteger = (CUInt(&H48) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:TSS Transmitter
            Public Const nxPropClst_FlexRayTSSTx As UInteger = (CUInt(&H49) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Wakeup Symbol Rx Idle
            Public Const nxPropClst_FlexRayWakeSymRxIdl As UInteger = (CUInt(&H50) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Wakeup Symbol Rx Low
            Public Const nxPropClst_FlexRayWakeSymRxLow As UInteger = (CUInt(&H51) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Wakeup Symbol Rx Window
            Public Const nxPropClst_FlexRayWakeSymRxWin As UInteger = (CUInt(&H52) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Wakeup Symbol Tx Idle
            Public Const nxPropClst_FlexRayWakeSymTxIdl As UInteger = (CUInt(&H53) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Wakeup Symbol Tx Low
            Public Const nxPropClst_FlexRayWakeSymTxLow As UInteger = (CUInt(&H54) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Cluster:FlexRay:Use Wakeup
            Public Const nxPropClst_FlexRayUseWakeup As UInteger = (CUInt(&H55) Or nxClass_Cluster Or nxPrptype_bool)
            '--rw
            ' Cluster:LIN:Schedules
            Public Const nxPropClst_LINSchedules As UInteger = (CUInt(&H70) Or nxClass_Cluster Or nxPrptype_1Dref)
            '--r
            ' Cluster:LIN:Tick
            Public Const nxPropClst_LINTick As UInteger = (CUInt(&H71) Or nxClass_Cluster Or nxPrptype_f64)
            '--rw
            ' Cluster:FlexRay:Allow Passive to Active
            Public Const nxPropClst_FlexRayAlwPassAct As UInteger = (CUInt(&H72) Or nxClass_Cluster Or nxPrptype_uint)
            '--rw
            ' Frame:Cluster
            Public Const nxPropFrm_ClusterRef As UInteger = (CUInt(&H1) Or nxClass_Frame Or nxPrptype_ref)
            '--r
            ' Frame:Comment
            Public Const nxPropFrm_Comment As UInteger = (CUInt(&H2) Or nxClass_Frame Or nxPrptype_string)
            '--rw
            ' Frame:Configuration Status
            Public Const nxPropFrm_ConfigStatus As UInteger = (CUInt(&H9) Or nxClass_Frame Or nxPrptype_uint)
            '--r
            ' Frame:Default Payload
            Public Const nxPropFrm_DefaultPayload As UInteger = (CUInt(&H5) Or nxClass_Frame Or nxPrptype_1Dbyte)
            '--rw
            ' Frame:Identifier (Slot)
            Public Const nxPropFrm_ID As UInteger = (CUInt(&H3) Or nxClass_Frame Or nxPrptype_uint)
            '--rw
            ' Frame:Name (Short)
            Public Const nxPropFrm_Name As UInteger = (CUInt(&H4) Or nxClass_Frame Or nxPrptype_string)
            '--rw
            ' Frame:Payload Length (in bytes)
            Public Const nxPropFrm_PayloadLen As UInteger = (CUInt(&H7) Or nxClass_Frame Or nxPrptype_uint)
            '--rw
            ' Frame:Signals
            Public Const nxPropFrm_SigRefs As UInteger = (CUInt(&H8) Or nxClass_Frame Or nxPrptype_1Dref)
            '--r
            ' Frame:CAN:Extended Identifier?
            Public Const nxPropFrm_CANExtID As UInteger = (CUInt(&H10) Or nxClass_Frame Or nxPrptype_bool)
            '--rw
            ' Frame:CAN:Timing Type
            Public Const nxPropFrm_CANTimingType As UInteger = (CUInt(&H11) Or nxClass_Frame Or nxPrptype_uint)
            '--rw
            ' Frame:CAN:Transmit Time
            Public Const nxPropFrm_CANTxTime As UInteger = (CUInt(&H12) Or nxClass_Frame Or nxPrptype_f64)
            '--rw
            ' Frame:FlexRay:Base Cycle
            Public Const nxPropFrm_FlexRayBaseCycle As UInteger = (CUInt(&H20) Or nxClass_Frame Or nxPrptype_uint)
            '--rw
            ' Frame:FlexRay:Channel Assignment
            Public Const nxPropFrm_FlexRayChAssign As UInteger = (CUInt(&H21) Or nxClass_Frame Or nxPrptype_uint)
            '--rw
            ' Frame:FlexRay:Cycle Repetition
            Public Const nxPropFrm_FlexRayCycleRep As UInteger = (CUInt(&H22) Or nxClass_Frame Or nxPrptype_uint)
            '--rw
            ' Frame:FlexRay:Payload Preamble?
            Public Const nxPropFrm_FlexRayPreamble As UInteger = (CUInt(&H23) Or nxClass_Frame Or nxPrptype_bool)
            '--rw
            ' Frame:FlexRay:Startup?
            Public Const nxPropFrm_FlexRayStartup As UInteger = (CUInt(&H24) Or nxClass_Frame Or nxPrptype_bool)
            '--rw
            ' Frame:FlexRay:Sync?
            Public Const nxPropFrm_FlexRaySync As UInteger = (CUInt(&H25) Or nxClass_Frame Or nxPrptype_bool)
            '--rw
            ' Frame:FlexRay:Timing Type
            Public Const nxPropFrm_FlexRayTimingType As UInteger = (CUInt(&H26) Or nxClass_Frame Or nxPrptype_uint)
            '--rw
            ' Frame:FlexRay:In Cycle Repetitions:Enabled?
            Public Const nxPropFrm_FlexRayInCycRepEnabled As UInteger = (CUInt(&H30) Or nxClass_Frame Or nxPrptype_bool)
            '--r
            ' Frame:FlexRay:In Cycle Repetitions:Identifiers
            Public Const nxPropFrm_FlexRayInCycRepIDs As UInteger = (CUInt(&H31) Or nxClass_Frame Or nxPrptype_1Duint)
            '--rw
            ' Frame:FlexRay:In Cycle Repetitions:Channel Assignments
            Public Const nxPropFrm_FlexRayInCycRepChAssigns As UInteger = (CUInt(&H32) Or nxClass_Frame Or nxPrptype_1Duint)
            '--rw
            ' Frame:LIN:Checksum
            Public Const nxPropFrm_LINChecksum As UInteger = (CUInt(&H50) Or nxClass_Frame Or nxPrptype_uint)
            '--r
            ' Frame:Mux:Is Data Multiplexed?
            Public Const nxPropFrm_MuxIsMuxed As UInteger = (CUInt(&H40) Or nxClass_Frame Or nxPrptype_bool)
            '--r
            ' Frame:Mux:Data Multiplexer Signal
            Public Const nxPropFrm_MuxDataMuxSigRef As UInteger = (CUInt(&H41) Or nxClass_Frame Or nxPrptype_ref)
            '--r
            ' Frame:Mux:Static Signals
            Public Const nxPropFrm_MuxStaticSigRefs As UInteger = (CUInt(&H42) Or nxClass_Frame Or nxPrptype_1Dref)
            '--r
            ' Frame:Mux:Subframes
            Public Const nxPropFrm_MuxSubframeRefs As UInteger = (CUInt(&H43) Or nxClass_Frame Or nxPrptype_1Dref)
            '--r
            ' Frame: PDUs
            Public Const nxPropFrm_PDURefs As UInteger = (CUInt(&H60) Or nxClass_Frame Or nxPrptype_1Dref)
            '--rw
            ' Frame: PDU Start Bits
            Public Const nxPropFrm_PDUStartBits As UInteger = (CUInt(&H61) Or nxClass_Frame Or nxPrptype_1Duint)
            '--rw
            ' Frame: PDU Update Bits
            Public Const nxPropFrm_PDUUpdateBits As UInteger = (CUInt(&H63) Or nxClass_Frame Or nxPrptype_1Duint)
            '--rw
            ' PDU: Cluster
            Public Const nxPropPDU_ClusterRef As UInteger = (CUInt(&H4) Or nxClass_PDU Or nxPrptype_ref)
            '--r
            ' PDU: Comment
            Public Const nxPropPDU_Comment As UInteger = (CUInt(&H2) Or nxClass_PDU Or nxPrptype_string)
            '--rw
            ' PDU:Configuration Status
            Public Const nxPropPDU_ConfigStatus As UInteger = (CUInt(&H7) Or nxClass_PDU Or nxPrptype_uint)
            '--r
            ' PDU:Frames
            Public Const nxPropPDU_FrmRefs As UInteger = (CUInt(&H6) Or nxClass_PDU Or nxPrptype_1Dref)
            '--r
            ' PDU: Name (Short)
            Public Const nxPropPDU_Name As UInteger = (CUInt(&H1) Or nxClass_PDU Or nxPrptype_string)
            '--rw
            ' PDU: Payload Length (in bytes)
            Public Const nxPropPDU_PayloadLen As UInteger = (CUInt(&H3) Or nxClass_PDU Or nxPrptype_uint)
            '--rw
            ' PDU:Signals
            Public Const nxPropPDU_SigRefs As UInteger = (CUInt(&H5) Or nxClass_PDU Or nxPrptype_1Dref)
            '--r
            ' PDU:Mux:Is Data Multiplexed?
            Public Const nxPropPDU_MuxIsMuxed As UInteger = (CUInt(&H8) Or nxClass_PDU Or nxPrptype_bool)
            '--r
            ' PDU:Mux:Data Multiplexer Signal
            Public Const nxPropPDU_MuxDataMuxSigRef As UInteger = (CUInt(&H9) Or nxClass_PDU Or nxPrptype_ref)
            '--r
            ' PDU:Mux:Static Signals
            Public Const nxPropPDU_MuxStaticSigRefs As UInteger = (CUInt(&HA) Or nxClass_PDU Or nxPrptype_1Dref)
            '--r
            ' PDU:Mux:Subframes
            Public Const nxPropPDU_MuxSubframeRefs As UInteger = (CUInt(&HB) Or nxClass_PDU Or nxPrptype_1Dref)
            '--r
            ' Signal:Byte Order
            Public Const nxPropSig_ByteOrdr As UInteger = (CUInt(&H1) Or nxClass_Signal Or nxPrptype_uint)
            '--rw
            ' Signal:Comment
            Public Const nxPropSig_Comment As UInteger = (CUInt(&H2) Or nxClass_Signal Or nxPrptype_string)
            '--rw
            ' Signal:Configuration Status
            Public Const nxPropSig_ConfigStatus As UInteger = (CUInt(&H9) Or nxClass_Signal Or nxPrptype_uint)
            '--r
            ' Signal:Data Type
            Public Const nxPropSig_DataType As UInteger = (CUInt(&H3) Or nxClass_Signal Or nxPrptype_uint)
            '--rw
            ' Signal:Default Value
            Public Const nxPropSig_Default As UInteger = (CUInt(&H4) Or nxClass_Signal Or nxPrptype_f64)
            '--rw
            ' Signal:Frame
            Public Const nxPropSig_FrameRef As UInteger = (CUInt(&H5) Or nxClass_Signal Or nxPrptype_ref)
            '--r
            ' Signal:Maximum Value
            Public Const nxPropSig_Max As UInteger = (CUInt(&H6) Or nxClass_Signal Or nxPrptype_f64)
            '--rw
            ' Signal:Minimum Value
            Public Const nxPropSig_Min As UInteger = (CUInt(&H7) Or nxClass_Signal Or nxPrptype_f64)
            '--rw
            ' Signal:Name (Short)
            Public Const nxPropSig_Name As UInteger = (CUInt(&H8) Or nxClass_Signal Or nxPrptype_string)
            '--rw
            ' Signal:Name (Unique to Cluster)
            Public Const nxPropSig_NameUniqueToCluster As UInteger = (CUInt(&H10) Or nxClass_Signal Or nxPrptype_string)
            '--r
            ' Signal:Number of Bits
            Public Const nxPropSig_NumBits As UInteger = (CUInt(&H12) Or nxClass_Signal Or nxPrptype_uint)
            '--rw
            ' Signal:PDU
            Public Const nxPropSig_PDURef As UInteger = (CUInt(&H11) Or nxClass_Signal Or nxPrptype_ref)
            '--r
            ' Signal:Scaling Factor
            Public Const nxPropSig_ScaleFac As UInteger = (CUInt(&H13) Or nxClass_Signal Or nxPrptype_f64)
            '--rw
            ' Signal:Scaling Offset
            Public Const nxPropSig_ScaleOff As UInteger = (CUInt(&H14) Or nxClass_Signal Or nxPrptype_f64)
            '--rw
            ' Signal:Start Bit
            Public Const nxPropSig_StartBit As UInteger = (CUInt(&H15) Or nxClass_Signal Or nxPrptype_uint)
            '--rw
            ' Signal:Unit
            Public Const nxPropSig_Unit As UInteger = (CUInt(&H16) Or nxClass_Signal Or nxPrptype_string)
            '--rw
            ' Signal:Mux:Data Multiplexer?
            Public Const nxPropSig_MuxIsDataMux As UInteger = (CUInt(&H30) Or nxClass_Signal Or nxPrptype_bool)
            '--rw
            ' Signal:Mux:Dynamic?
            Public Const nxPropSig_MuxIsDynamic As UInteger = (CUInt(&H31) Or nxClass_Signal Or nxPrptype_bool)
            '--r
            ' Signal:Mux:Multiplexer Value
            Public Const nxPropSig_MuxValue As UInteger = (CUInt(&H32) Or nxClass_Signal Or nxPrptype_uint)
            '--r
            ' Signal:Mux:Subframe
            Public Const nxPropSig_MuxSubfrmRef As UInteger = (CUInt(&H33) Or nxClass_Signal Or nxPrptype_ref)
            '--r
            ' Subframe:Configuration Status
            Public Const nxPropSubfrm_ConfigStatus As UInteger = (CUInt(&H9) Or nxClass_Subframe Or nxPrptype_uint)
            '--r
            ' Subframe:Dynamic Signals
            Public Const nxPropSubfrm_DynSigRefs As UInteger = (CUInt(&H1) Or nxClass_Subframe Or nxPrptype_1Dref)
            '--r  
            ' Subframe:"Frame"
            Public Const nxPropSubfrm_FrmRef As UInteger = (CUInt(&H2) Or nxClass_Subframe Or nxPrptype_ref)
            '--r
            ' Subframe:Multiplexer Value
            Public Const nxPropSubfrm_MuxValue As UInteger = (CUInt(&H3) Or nxClass_Subframe Or nxPrptype_uint)
            '--rw
            ' Subframe:Name (Short)
            Public Const nxPropSubfrm_Name As UInteger = (CUInt(&H4) Or nxClass_Subframe Or nxPrptype_string)
            '--rw
            ' Subframe:PDU
            Public Const nxPropSubfrm_PDURef As UInteger = (CUInt(&H5) Or nxClass_Subframe Or nxPrptype_ref)
            '--r
            ' Subframe:Name (Unique to Cluster)
            Public Const nxPropSubfrm_NameUniqueToCluster As UInteger = (CUInt(&H7) Or nxClass_Subframe Or nxPrptype_string)
            '--r
            ' ECU:Cluster
            Public Const nxPropECU_ClstRef As UInteger = (CUInt(&H1) Or nxClass_ECU Or nxPrptype_ref)
            '--r
            ' ECU:Comment
            Public Const nxPropECU_Comment As UInteger = (CUInt(&H5) Or nxClass_ECU Or nxPrptype_string)
            '--rw
            ' ECU:Configuration Status
            Public Const nxPropECU_ConfigStatus As UInteger = (CUInt(&H9) Or nxClass_ECU Or nxPrptype_uint)
            '--r
            ' ECU:Name (Short)
            Public Const nxPropECU_Name As UInteger = (CUInt(&H2) Or nxClass_ECU Or nxPrptype_string)
            '--rw
            ' ECU:Frames Received
            Public Const nxPropECU_RxFrmRefs As UInteger = (CUInt(&H3) Or nxClass_ECU Or nxPrptype_1Dref)
            '--rw
            ' ECU:Frames Transmitted
            Public Const nxPropECU_TxFrmRefs As UInteger = (CUInt(&H4) Or nxClass_ECU Or nxPrptype_1Dref)
            '--rw
            ' ECU:FlexRay:Coldstart?
            Public Const nxPropECU_FlexRayIsColdstart As UInteger = (CUInt(&H10) Or nxClass_ECU Or nxPrptype_bool)
            '--r
            ' ECU:FlexRay:Startup Frame
            Public Const nxPropECU_FlexRayStartupFrameRef As UInteger = (CUInt(&H11) Or nxClass_ECU Or nxPrptype_ref)
            '--r
            ' ECU:FlexRay:Wakeup Pattern
            Public Const nxPropECU_FlexRayWakeupPtrn As UInteger = (CUInt(&H12) Or nxClass_ECU Or nxPrptype_uint)
            '--rw
            ' ECU:FlexRay:Wakeup Channels
            Public Const nxPropECU_FlexRayWakeupChs As UInteger = (CUInt(&H13) Or nxClass_ECU Or nxPrptype_uint)
            '--rw
            ' ECU:FlexRay:Connected Channels
            Public Const nxPropECU_FlexRayConnectedChs As UInteger = (CUInt(&H14) Or nxClass_ECU Or nxPrptype_uint)
            '--rw
            ' ECU:LIN:Master?
            Public Const nxPropECU_LINMaster As UInteger = (CUInt(&H20) Or nxClass_ECU Or nxPrptype_bool)
            '--rw
            ' ECU:LIN:Protocol Version
            Public Const nxPropECU_LINProtocolVer As UInteger = (CUInt(&H21) Or nxClass_ECU Or nxPrptype_uint)
            '--rw
            ' ECU:LIN:Initial NAD
            Public Const nxPropECU_LINInitialNAD As UInteger = (CUInt(&H22) Or nxClass_ECU Or nxPrptype_uint)
            '--rw
            ' ECU:LIN:Configured NAD
            Public Const nxPropECU_LINConfigNAD As UInteger = (CUInt(&H23) Or nxClass_ECU Or nxPrptype_uint)
            '--rw
            ' ECU:LIN:Supplier ID
            Public Const nxPropECU_LINSupplierID As UInteger = (CUInt(&H24) Or nxClass_ECU Or nxPrptype_uint)
            '--rw
            ' ECU:LIN:Function ID
            Public Const nxPropECU_LINFunctionID As UInteger = (CUInt(&H25) Or nxClass_ECU Or nxPrptype_uint)
            '--rw
            ' ECU:LIN:P2 Min
            Public Const nxPropECU_LINP2min As UInteger = (CUInt(&H26) Or nxClass_ECU Or nxPrptype_f64)
            '--rw
            ' ECU:LIN:ST Min
            Public Const nxPropECU_LINSTmin As UInteger = (CUInt(&H27) Or nxClass_ECU Or nxPrptype_f64)
            '--rw
            ' LIN Schedule:Cluster
            Public Const nxPropLINSched_ClstRef As UInteger = (CUInt(&H5) Or nxClass_LINSched Or nxPrptype_ref)
            '--r
            ' LIN Schedule:Comment
            Public Const nxPropLINSched_Comment As UInteger = (CUInt(&H6) Or nxClass_LINSched Or nxPrptype_string)
            '--rw
            ' LIN Schedule:Configuration Status
            Public Const nxPropLINSched_ConfigStatus As UInteger = (CUInt(&H7) Or nxClass_LINSched Or nxPrptype_uint)
            '--r
            ' LIN Schedule:Entries
            Public Const nxPropLINSched_Entries As UInteger = (CUInt(&H1) Or nxClass_LINSched Or nxPrptype_1Dref)
            '--r
            ' LIN Schedule:Name
            Public Const nxPropLINSched_Name As UInteger = (CUInt(&H2) Or nxClass_LINSched Or nxPrptype_string)
            '--rw
            ' LIN Schedule:Priority
            Public Const nxPropLINSched_Priority As UInteger = (CUInt(&H3) Or nxClass_LINSched Or nxPrptype_uint)
            '--rw
            ' LIN Schedule:Run Mode
            Public Const nxPropLINSched_RunMode As UInteger = (CUInt(&H4) Or nxClass_LINSched Or nxPrptype_uint)
            '--rw
            ' LIN Schedule Entry:Collision Resolving Schedule
            Public Const nxPropLINSchedEntry_CollisionResSched As UInteger = (CUInt(&H1) Or nxClass_LINSchedEntry Or nxPrptype_ref)
            '--rw
            ' LIN Schedule Entry:Delay
            Public Const nxPropLINSchedEntry_Delay As UInteger = (CUInt(&H2) Or nxClass_LINSchedEntry Or nxPrptype_f64)
            '--rw
            ' LIN Schedule Entry:Event Identifier
            Public Const nxPropLINSchedEntry_EventID As UInteger = (CUInt(&H3) Or nxClass_LINSchedEntry Or nxPrptype_uint)
            '--rw
            ' LIN Schedule Entry:Frames
            Public Const nxPropLINSchedEntry_Frames As UInteger = (CUInt(&H4) Or nxClass_LINSchedEntry Or nxPrptype_1Dref)
            '--rw
            ' LIN Schedule Entry:Name
            Public Const nxPropLINSchedEntry_Name As UInteger = (CUInt(&H6) Or nxClass_LINSchedEntry Or nxPrptype_string)
            '--rw
            ' LIN Schedule Entry:Name (Unique to Cluster)
            Public Const nxPropLINSchedEntry_NameUniqueToCluster As UInteger = (CUInt(&H8) Or nxClass_LINSchedEntry Or nxPrptype_string)
            '--r
            ' LIN Schedule Entry:Schedule
            Public Const nxPropLINSchedEntry_Sched As UInteger = (CUInt(&H7) Or nxClass_LINSchedEntry Or nxPrptype_ref)
            '--r
            ' LIN Schedule Entry:Type
            Public Const nxPropLINSchedEntry_Type As UInteger = (CUInt(&H5) Or nxClass_LINSchedEntry Or nxPrptype_uint)
            '--rw
            ' LIN Schedule Entry:Node Configuration:Free Format:Data Bytes
            Public Const nxPropLINSchedEntry_NC_FF_DataBytes As UInteger = (CUInt(&H9) Or nxClass_LINSchedEntry Or nxPrptype_1Dbyte)
            '--rw

            '**********************************************************************
            '               C O N S T A N T S   F O R   F U N C T I O N   P A R A M E T E R S
            '            **********************************************************************


            ' Parameter Mode of function nxCreateSession
            Public Const nxMode_SignalInSinglePoint As Integer = 0
            ' SignalInSinglePoint
            Public Const nxMode_SignalInWaveform As Integer = 1
            ' SignalInWaveform
            Public Const nxMode_SignalInXY As Integer = 2
            ' SignalInXY
            Public Const nxMode_SignalOutSinglePoint As Integer = 3
            ' SignalOutSinglePoint
            Public Const nxMode_SignalOutWaveform As Integer = 4
            ' SignalOutWaveform
            Public Const nxMode_SignalOutXY As Integer = 5
            ' SignalOutXY
            Public Const nxMode_FrameInStream As Integer = 6
            ' FrameInStream
            Public Const nxMode_FrameInQueued As Integer = 7
            ' FrameInQueued
            Public Const nxMode_FrameInSinglePoint As Integer = 8
            ' FrameInSinglePoint
            Public Const nxMode_FrameOutStream As Integer = 9
            ' FrameOutStream
            Public Const nxMode_FrameOutQueued As Integer = 10
            ' FrameOutQueued
            Public Const nxMode_FrameOutSinglePoint As Integer = 11
            ' FrameOutSinglePoint
            Public Const nxMode_SignalConversionSinglePoint As Integer = 12
            ' SignalConversionSinglePoint
            ' Parameter Scope of functions nxStart, nxStop
            Public Const nxStartStop_Normal As Integer = 0
            ' StartStop_Normal
            Public Const nxStartStop_SessionOnly As Integer = 1
            ' StartStop_SessionOnly
            Public Const nxStartStop_InterfaceOnly As Integer = 2
            ' StartStop_InterfaceOnly
            Public Const nxStartStop_SessionOnlyBlocking As Integer = 3
            ' StartStop_SessionOnlyBlocking
            ' Parameter Modifier of nxBlink
            Public Const nxBlink_Disable As Integer = 0
            ' Blink_Disable
            Public Const nxBlink_Enable As Integer = 1
            ' Blink_Enable
            ' Terminal names for nxConnectTerminals and nxDisconnectTerminals (source or destination)
            Public Const nxTerm_PXI_Trig0 As String = "PXI_Trig0"
            ' PXI_Trig0 same as RTSI0
            Public Const nxTerm_PXI_Trig1 As String = "PXI_Trig1"
            Public Const nxTerm_PXI_Trig2 As String = "PXI_Trig2"
            Public Const nxTerm_PXI_Trig3 As String = "PXI_Trig3"
            Public Const nxTerm_PXI_Trig4 As String = "PXI_Trig4"
            Public Const nxTerm_PXI_Trig5 As String = "PXI_Trig5"
            Public Const nxTerm_PXI_Trig6 As String = "PXI_Trig6"
            Public Const nxTerm_PXI_Trig7 As String = "PXI_Trig7"
            Public Const nxTerm_FrontPanel0 As String = "FrontPanel0"
            Public Const nxTerm_FrontPanel1 As String = "FrontPanel1"
            Public Const nxTerm_PXI_Star As String = "PXI_Star"
            Public Const nxTerm_PXI_Clk10 As String = "PXI_Clk10"
            Public Const nxTerm_10MHzTimebase As String = "10MHzTimebase"
            Public Const nxTerm_1MHzTimebase As String = "1MHzTimebase"
            Public Const nxTerm_MasterTimebase As String = "MasterTimebase"
            Public Const nxTerm_CommTrigger As String = "CommTrigger"
            Public Const nxTerm_StartTrigger As String = "StartTrigger"
            Public Const nxTerm_FlexRayStartCycle As String = "FlexRayStartCycle"
            Public Const nxTerm_FlexRayMacrotick As String = "FlexRayMacrotick"
            Public Const nxTerm_LogTrigger As String = "LogTrigger"

            ' StateID for nxReadState
            '            These constants use an encoding similar to property ID (nxProp_ prefix).
            '            

            ' Current time of the interface (using nxTimestamp_t)
            Public Const nxState_TimeCurrent As UInteger = (CUInt(&H1) Or nxClass_Interface Or nxPrptype_time)
            ' TimeCurrent
            ' Time when communication began on the interface (protocol operational / integrated)
            Public Const nxState_TimeCommunicating As UInteger = (CUInt(&H2) Or nxClass_Interface Or nxPrptype_time)
            ' TimeCommunicating
            ' Start time of the interface, when the attempt to communicate began (startup protocol)
            Public Const nxState_TimeStart As UInteger = (CUInt(&H3) Or nxClass_Interface Or nxPrptype_time)
            ' TimeStart
            ' Session information: Use macros with prefix nxSessionInfo_Get_ to get fields of the uint
            Public Const nxState_SessionInfo As UInteger = (CUInt(&H4) Or nxClass_Interface Or nxPrptype_uint)
            ' SessionInfo
            ' CAN communication: Use macros with prefix nxCANComm_Get_ to get fields of the uint
            Public Const nxState_CANComm As UInteger = (CUInt(&H10) Or nxClass_Interface Or nxPrptype_uint)
            ' CANComm
            ' FlexRay communication: Use macros with prefix nxFlexRayComm_Get_ to get fields of the uint
            Public Const nxState_FlexRayComm As UInteger = (CUInt(&H20) Or nxClass_Interface Or nxPrptype_uint)
            ' FlexRayComm
            ' FlexRay statistics: Use typedef nxFlexRayStats_t to read these statistics using a struct of multiple uint
            Public Const nxState_FlexRayStats As UInteger = (CUInt(&H21) Or nxClass_Interface Or nxPrptype_1Duint)
            ' FlexRayStats
            ' LIN communication: Use macros with prefix nxLINComm_Get_ to get fields of 2 uint's
            Public Const nxState_LINComm As UInteger = (CUInt(&H30) Or nxClass_Interface Or nxPrptype_uint)
            ' LINComm
            ' StateID for nxWriteState
            '            These constants use an encoding similar to property ID (nxProp_ prefix).
            '            

            Public Const nxState_LINScheduleChange As UInteger = (CUInt(&H81) Or nxClass_Interface Or nxPrptype_uint)
            ' LINScheduleChange
            Public Const nxState_LINDiagnosticScheduleChange As UInteger = (CUInt(&H83) Or nxClass_Interface Or nxPrptype_uint)
            ' LINDiagnosticScheduleChange
            Public Const nxState_FlexRaySymbol As UInteger = (CUInt(&H82) Or nxClass_Interface Or nxPrptype_uint)
            ' FlexRaySymbol

            ' State of frames in the session, from nxState_SessionInfo (nxSessionInfo_Get_State)
            Public Const nxSessionInfoState_Stopped As UInteger = 0
            ' all frames stopped
            Public Const nxSessionInfoState_Started As UInteger = 1
            ' all frames started
            Public Const nxSessionInfoState_Mix As UInteger = 2
            ' one or more frames started, and one or more frames stopped
            ' Communication state from nxState_CANComm (nxCANComm_Get_CommState)
            Public Const nxCANCommState_ErrorActive As Integer = 0
            Public Const nxCANCommState_ErrorPassive As Integer = 1
            Public Const nxCANCommState_BusOff As Integer = 2
            Public Const nxCANCommState_Init As Integer = 3

            ' Last bus error from nxState_CANComm (nxCANComm_Get_LastErr)
            Public Const nxCANLastErr_None As Integer = 0
            Public Const nxCANLastErr_Stuff As Integer = 1
            Public Const nxCANLastErr_Form As Integer = 2
            Public Const nxCANLastErr_Ack As Integer = 3
            Public Const nxCANLastErr_Bit1 As Integer = 4
            Public Const nxCANLastErr_Bit0 As Integer = 5
            Public Const nxCANLastErr_CRC As Integer = 6

            ' Macros to get fields of uint returned by nxReadState of nxState_FlexRayComm
            ' Get FlexRay Protocol Operation Control (POC) state,
            '            which uses constants with prefix nxFlexRayPOCState_ 

            Public Function nxFlexRayComm_Get_POCState(ByVal StateValue As UInteger) As Byte
                Return CByte(CUInt(StateValue) And &HF)
            End Function
            ' From FlexRay spec 9.3.1.3.4: "the number of consecutive even/odd cycle pairs
            '        (vClockCorrectionFailed) that have passed without clock synchronization having performed an offset or a rate
            '        correction due to lack of synchronization frames (as maintained by the POC process)."
            '        This value is used for comparison to the cluster thresholds MaxWithoutClockCorrectFatal and
            '        MaxWithoutClockCorrectionPassive (XNET properties nxPropClst_FlexRayMaxWoClkCorFat
            '        and nxPropClst_FlexRayMaxWoClkCorPas). 

            Public Function nxFlexRayComm_Get_ClockCorrFailed(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 4) And &HF)
            End Function
            ' From FlexRay spec 9.3.1.3.1: "the number of consecutive even/odd cycle pairs (vAllowPassiveToActive)
            '            that have passed with valid rate and offset correction terms, but the node still in POC:normal passive
            '            state due to a host configured delay to POC:normal active state (as maintained by the POC process).
            '            This value is used for comparison to the interface threshold AllowPassiveToActive
            '            (XNET property nxPropSession_IntfFlexRayAlwPassAct). 

            Public Function nxFlexRayComm_Get_PassiveToActiveCount(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 8) And &H1F)
            End Function
            Public Function nxFlexRayComm_Get_ChannelASleep(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 13) And &H1)
            End Function
            Public Function nxFlexRayComm_Get_ChannelBSleep(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 14) And &H1)
            End Function

            ' POC state (Protocol Operation Control state) from nxFlexRayPOC_Get_State
            Public Const nxFlexRayPOCState_DefaultConfig As Integer = 0
            Public Const nxFlexRayPOCState_Ready As Integer = 1
            Public Const nxFlexRayPOCState_NormalActive As Integer = 2
            Public Const nxFlexRayPOCState_NormalPassive As Integer = 3
            Public Const nxFlexRayPOCState_Halt As Integer = 4
            Public Const nxFlexRayPOCState_Monitor As Integer = 5
            Public Const nxFlexRayPOCState_Config As Integer = 15

            ' Macros to get fields of 1st uint returned by nxReadState of nxState_LINComm
            ' Get indication of LIN interface sleep state; 1 = asleep, 0 = awake
            Public Function nxLINComm_Get_Sleep(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 1) And &H1)
            End Function
            ' Get LIN communication state; uses constants with prefix nxLINCommState_
            Public Function nxLINComm_Get_CommState(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 2) And &H3)
            End Function
            ' Get last bus error; this code uses constants with prefix nxLINLastErrCode_
            Public Function nxLINComm_Get_LastErrCode(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 4) And &HF)
            End Function
            ' Get received data for last bus error; this value applies only to specific codes
            Public Function nxLINComm_Get_LastErrReceived(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 8) And &HFF)
            End Function
            ' Get expected data for last bus error; this value applies only to specific codes
            Public Function nxLINComm_Get_LastErrExpected(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 16) And &HFF)
            End Function
            ' Get ID of last bus error; this value applies only to specific codes
            Public Function nxLINComm_Get_LastErrID(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 24) And &H3F)
            End Function
            ' Get indication of LIN transceiver ready (powered); 1 = powered, 0 = not powered
            Public Function nxLINComm_Get_TcvrRdy(ByVal StateValue As UInteger) As Byte
                Return CByte((CUInt(StateValue) >> 31) And &H1)
            End Function

            ' Macros to get fields of 2nd uint returned by nxReadState of nxState_LINComm
            ' Get index of the currently running schedule (0xFF if Null-schedule).
            Public Function nxLINComm_Get2_ScheduleIndex(ByVal State2Value As UInteger) As Byte
                Return CByte(CUInt(State2Value) And &HFF)
            End Function

            ' Communication state from nxState_LINComm (nxLINComm_Get_CommState macro)
            Public Const nxLINCommState_Idle As Integer = 0
            Public Const nxLINCommState_Active As Integer = 1
            Public Const nxLINCommState_Inactive As Integer = 2

            ' Diagnostic schedule state for nxState_LINDiagnosticScheduleChange
            Public Const nxLINDiagnosticSchedule_NULL As Integer = &H0
            Public Const nxLINDiagnosticSchedule_MasterReq As Integer = &H1
            Public Const nxLINDiagnosticSchedule_SlaveResp As Integer = &H2

            ' Last error code from nxState_LINComm (nxLINComm_Get_LastErrCode macro)
            Public Const nxLINLastErrCode_None As Integer = 0
            Public Const nxLINLastErrCode_UnknownId As Integer = 1
            Public Const nxLINLastErrCode_Form As Integer = 2
            Public Const nxLINLastErrCode_Framing As Integer = 3
            Public Const nxLINLastErrCode_Readback As Integer = 4
            Public Const nxLINLastErrCode_Timeout As Integer = 5
            Public Const nxLINLastErrCode_CRC As Integer = 6

            ' Condition of nxWait
            Public Const nxCondition_TransmitComplete As Integer = &H8001
            ' TransmitComplete
            Public Const nxCondition_IntfCommunicating As Integer = &H8002
            ' IntfCommunicating
            Public Const nxCondition_IntfRemoteWakeup As Integer = &H8003
            ' IntfRemoteWakeup
            ' Constants for use with Timeout parameter of read and write functions
            Public Const nxTimeout_None As Integer = (0)
            Public Const nxTimeout_Infinite As Integer = (-1)

            ' Parameter Mode of function nxdbGetDBCAttribute and nxdbGetDBCAttributeSize
            Public Const nxGetDBCMode_Attribute As Integer = 0
            ' Attribute
            Public Const nxGetDBCMode_EnumerationList As Integer = 1
            ' Enumeration List
            Public Const nxGetDBCMode_AttributeList As Integer = 2
            ' List of available attributes of given type
            Public Const nxGetDBCMode_ValueTableList As Integer = 3
            ' Value table for a signal
            '**********************************************************************
            '               C O N S T A N T S   F O R   H A R D W A R E   P R O P E R T I E S
            '            **********************************************************************


            ' System/Device/Interface properties (hardware info)

            ' Property ID nxPropSys_VerPhase
            Public Const nxPhase_Development As Integer = 0
            Public Const nxPhase_Alpha As Integer = 1
            Public Const nxPhase_Beta As Integer = 2
            Public Const nxPhase_Release As Integer = 3

            ' Property ID nxPropDev_FormFac
            Public Const nxDevForm_PXI As Integer = 0
            Public Const nxDevForm_PCI As Integer = 1
            Public Const nxDevForm_cSeries As Integer = 2

            ' Property ID nxPropIntf_CANTermCap
            Public Const nxCANTermCap_No As Integer = 0
            Public Const nxCANTermCap_Yes As Integer = 1

            ' Property ID nxPropIntf_CANTcvrCap
            Public Const nxCANTcvrCap_HS As Integer = 0
            Public Const nxCANTcvrCap_LS As Integer = 1
            Public Const nxCANTcvrCap_XS As Integer = 3

            ' Property ID nxPropIntf_Protocol and nxPropClst_Protocol
            Public Const nxProtocol_CAN As Integer = 0
            Public Const nxProtocol_FlexRay As Integer = 1
            Public Const nxProtocol_LIN As Integer = 2

            '**********************************************************************
            '               C O N S T A N T S   F O R   S E S S I O N   P R O P E R T I E S
            '            **********************************************************************


            ' Session properties (including runtime interface properties)

            ' Macro to set nxPropSession_IntfBaudRate for an advanced CAN baud rate (bit timings)
            ' If you pass a basic baud rate like 125000 or 500000, NI-XNET calculates bit timings for you
            Public Function nxAdvCANBaudRate_Set(ByVal TimeQuantum As UInteger, ByVal TimeSeg0 As UInteger, ByVal TimeSeg1 As UInteger, ByVal SyncJumpWidth As UInteger) As UInteger
                Return ((CUInt(TimeQuantum) And &HFFFF) Or ((CUInt(TimeSeg0) << 16) And &HF0000) Or ((CUInt(TimeSeg1) << 20) And &H700000) Or ((CUInt(SyncJumpWidth) << 24) And &H3000000) Or CUInt(&H80000000UI))
            End Function

            ' Macros to get fields of nxPropSession_IntfBaudRate for an advanced CAN baud rate
            Public Function nxAdvCANBaudRate_Get_TimeQuantum(ByVal AdvBdRt As UInteger) As UShort
                Return CUShort(CUInt(AdvBdRt) And &HFFFF)
            End Function
            Public Function nxAdvCANBaudRate_Get_TimeSeg0(ByVal AdvBdRt As UInteger) As Byte
                Return CByte((CUInt(AdvBdRt) >> 16) And &HF)
            End Function
            Public Function nxAdvCANBaudRate_Get_TimeSeg1(ByVal AdvBdRt As UInteger) As Byte
                Return CByte((CUInt(AdvBdRt) >> 20) And &H7)
            End Function
            Public Function nxAdvCANBaudRate_Get_SyncJumpWidth(ByVal AdvBdRt As UInteger) As Byte
                Return CByte((CUInt(AdvBdRt) >> 24) And &H3)
            End Function
            Public Function nxAdvCANBaudRate_Get_NumSamples(ByVal AdvBdRt As UInteger) As Byte
                Return CByte((CUInt(AdvBdRt) >> 26) And &H1)
            End Function

            ' Property ID nxPropSession_IntfCANTerm
            Public Const nxCANTerm_Off As Integer = 0
            Public Const nxCANTerm_On As Integer = 1

            ' Property ID nxPropSession_IntfCANTcvrState
            Public Const nxCANTcvrState_Normal As Integer = 0
            Public Const nxCANTcvrState_Sleep As Integer = 1
            Public Const nxCANTcvrState_SWWakeup As Integer = 2
            Public Const nxCANTcvrState_SWHighSpeed As Integer = 3

            ' Property ID nxPropSession_IntfCANTcvrType
            Public Const nxCANTcvrType_HS As Integer = 0
            Public Const nxCANTcvrType_LS As Integer = 1
            Public Const nxCANTcvrType_SW As Integer = 2
            Public Const nxCANTcvrType_Ext As Integer = 3
            Public Const nxCANTcvrType_Disc As Integer = 4

            ' Property ID nxPropSession_IntfFlexRaySampPerMicro
            Public Const nxFlexRaySampPerMicro_1 As Integer = 0
            Public Const nxFlexRaySampPerMicro_2 As Integer = 1
            Public Const nxFlexRaySampPerMicro_4 As Integer = 2

            ' Property ID nxPropSession_IntfFlexRayTerm
            Public Const nxFlexRayTerm_Off As Integer = 0
            Public Const nxFlexRayTerm_On As Integer = 1

            ' Property ID nxPropSession_IntfLINSleep
            Public Const nxLINSleep_RemoteSleep As Integer = 0
            Public Const nxLINSleep_RemoteWake As Integer = 1
            Public Const nxLINSleep_LocalSleep As Integer = 2
            Public Const nxLINSleep_LocalWake As Integer = 3

            ' Property ID nxPropSession_IntfLINTerm
            Public Const nxLINTerm_Off As Integer = 0
            Public Const nxLINTerm_On As Integer = 1

            ' Property ID nxPropSession_IntfOutStrmTimng
            Public Const nxOutStrmTimng_Immediate As Integer = 0
            Public Const nxOutStrmTimng_ReplayExclusive As Integer = 1
            Public Const nxOutStrmTimng_ReplayInclusive As Integer = 2

            ' Property ID nxPropSession_IntfCANPendTxOrder
            Public Const nxCANPendTxOrder_AsSubmitted As Integer = 0
            Public Const nxCANPendTxOrder_ByIdentifier As Integer = 1

            ' Property ID nxPropSession_IntfFlexRaySleep
            Public Const nxFlexRaySleep_LocalSleep As Integer = 0
            Public Const nxFlexRaySleep_LocalWake As Integer = 1
            Public Const nxFlexRaySleep_RemoteWake As Integer = 2

            ' Property ID nxPropSession_IntfCANExtTcvrConfig
            ' These bits can be combined to define the capabilities of the connected
            ' external transceiver.
            Public Const nxCANExtTcvrConfig_NormalSupported As Integer = (1 << 2)
            Public Const nxCANExtTcvrConfig_SleepSupported As Integer = (1 << 5)
            Public Const nxCANExtTcvrConfig_SWWakeupSupported As Integer = (1 << 8)
            Public Const nxCANExtTcvrConfig_SWHighSpeedSupported As Integer = (1 << 11)
            Public Const nxCANExtTcvrConfig_PowerOnSupported As Integer = (1 << 14)
            Public Const nxCANExtTcvrConfig_NormalOutput0Set As Integer = (1 << 0)
            Public Const nxCANExtTcvrConfig_SleepOutput0Set As Integer = (1 << 3)
            Public Const nxCANExtTcvrConfig_SWWakeupOutput0Set As Integer = (1 << 6)
            Public Const nxCANExtTcvrConfig_SWHighSpeedOutput0Set As Integer = (1 << 9)
            Public Const nxCANExtTcvrConfig_PowerOnOutput0Set As Integer = (1 << 12)
            Public Const nxCANExtTcvrConfig_NormalOutput1Set As Integer = (1 << 1)
            Public Const nxCANExtTcvrConfig_SleepOutput1Set As Integer = (1 << 4)
            Public Const nxCANExtTcvrConfig_SWWakeupOutput1Set As Integer = (1 << 7)
            Public Const nxCANExtTcvrConfig_SWHighSpeedOutput1Set As Integer = (1 << 10)
            Public Const nxCANExtTcvrConfig_PowerOnOutput1Set As Integer = (1 << 13)
            Public Const nxCANExtTcvrConfig_nErrConnected As Integer = (1 << 31)


            '**********************************************************************
            '               C O N S T A N T S   F O R   D A T A B A S E   P R O P E R T I E S
            '            **********************************************************************


            ' Database properties (Database/Cluster/ECU/Frame/Subframe/Signal)

            ' Property ID nxPropClst_FlexRayChannels, nxPropFrm_FlexRayChAssign
            ' nxPropClst_FlexRayConnectedChannels and nxPropClst_FlexRayWakeChannels
            Public Const nxFrmFlexRayChAssign_A As Integer = 1
            Public Const nxFrmFlexRayChAssign_B As Integer = 2
            Public Const nxFrmFlexRayChAssign_AandB As Integer = 3
            Public Const nxFrmFlexRayChAssign_None As Integer = 4

            ' Property ID nxPropClst_FlexRaySampClkPer
            Public Const nxClstFlexRaySampClkPer_p0125us As Integer = 0
            Public Const nxClstFlexRaySampClkPer_p025us As Integer = 1
            Public Const nxClstFlexRaySampClkPer_p05us As Integer = 2

            ' Property ID nxPropClst_Protocol uses prefix nxProtocol_

            ' Property ID nxPropFrm_FlexRayTimingType
            Public Const nxFrmFlexRayTiming_Cyclic As Integer = 0
            Public Const nxFrmFlexRayTiming_Event As Integer = 1

            ' Property ID nxPropFrm_CANTimingType
            Public Const nxFrmCANTiming_CyclicData As Integer = 0
            Public Const nxFrmCANTiming_EventData As Integer = 1
            Public Const nxFrmCANTiming_CyclicRemote As Integer = 2
            Public Const nxFrmCANTiming_EventRemote As Integer = 3

            ' Property ID nxPropSig_ByteOrdr
            Public Const nxSigByteOrdr_LittleEndian As Integer = 0
            ' Intel
            Public Const nxSigByteOrdr_BigEndian As Integer = 1
            ' Motorola
            ' Property ID nxPropSig_DataType
            Public Const nxSigDataType_Signed As Integer = 0
            Public Const nxSigDataType_Unsigned As Integer = 1
            Public Const nxSigDataType_IEEEFloat As Integer = 2

            ' Property ID nxPropECU_LINProtocolVer
            Public Const nxLINProtocolVer_1_2 As Integer = 2
            Public Const nxLINProtocolVer_1_3 As Integer = 3
            Public Const nxLINProtocolVer_2_0 As Integer = 4
            Public Const nxLINProtocolVer_2_1 As Integer = 5
            Public Const nxLINProtocolVer_2_2 As Integer = 6

            ' Property ID nxPropLINSched_RunMode
            Public Const nxLINSchedRunMode_Continuous As Integer = 0
            Public Const nxLINSchedRunMode_Once As Integer = 1
            Public Const nxLINSchedRunMode_Null As Integer = 2

            ' Property ID nxPropLINSchedEntry_Type
            Public Const nxLINSchedEntryType_Unconditional As Integer = 0
            Public Const nxLINSchedEntryType_Sporadic As Integer = 1
            Public Const nxLINSchedEntryType_EventTriggered As Integer = 2
            Public Const nxLINSchedEntryType_NodeConfigService As Integer = 3

            ' Property ID nxPropFrm_LINChecksum
            Public Const nxFrmLINChecksum_Classic As Integer = 0
            Public Const nxFrmLINChecksum_Enhanced As Integer = 1

            '**********************************************************************
            '                                    F R A M E
            '            **********************************************************************


            ' Type
            Public Const nxFrameType_CAN_Data As UInteger = &H0
            Public Const nxFrameType_CAN_Remote As UInteger = &H1
            Public Const nxFrameType_CAN_BusError As UInteger = &H2
            Public Const nxFrameType_FlexRay_Data As UInteger = &H20
            Public Const nxFrameType_FlexRay_Null As UInteger = &H21
            Public Const nxFrameType_FlexRay_Symbol As UInteger = &H22
            Public Const nxFrameType_LIN_Data As UInteger = &H40
            Public Const nxFrameType_LIN_BusError As UInteger = &H41
            Public Const nxFrameType_Special_Delay As UInteger = &HE0
            Public Const nxFrameType_Special_LogTrigger As UInteger = &HE1
            Public Const nxFrameType_Special_StartTrigger As UInteger = &HE2


            ' For Data frames, your application may not be concerned with specifics for
            '            CAN, FlexRay, or LIN. For example, you can use fields of the frame to determine
            '            the contents of Payload, and write general-purpose code to map signal
            '            values in/out of the Payload data bytes.
            '            This macro can be used with the frame's Type to determine if the frame is a
            '            data frame. The macro is used in boolean conditionals. 

            '        public byte nxFrameType_IsData(byte frametype)
            Public Function nxFrameType_IsData(ByVal frametype As Byte) As Boolean
                Return (CByte(frametype) And &H1F) = 0
            End Function

            Public Const nxFrameId_CAN_IsExtended As UInteger = &H20000000

            ' Macros to get fields of frame Identifier for FlexRay input
            Public Function nxFrameId_FlexRay_Get_Slot(ByVal FrameId As UInteger) As UShort
                Return CUShort(CUInt(FrameId) And &HFFFF)
            End Function

            ' When Type is nxFrameType_FlexRay_Data,
            '            the following bitmasks are used with the Flags field.
            '            

            Public Const nxFrameFlags_FlexRay_Startup As Byte = &H1
            ' Startup frame
            Public Const nxFrameFlags_FlexRay_Sync As Byte = &H2
            ' Sync frame
            Public Const nxFrameFlags_FlexRay_Preamble As Byte = &H4
            ' Preamble bit
            Public Const nxFrameFlags_FlexRay_ChA As Byte = &H10
            ' Transfer on Channel A
            Public Const nxFrameFlags_FlexRay_ChB As Byte = &H20
            ' Transfer on Channel B
            ' When Type is nxFrameType_LIN_Data,
            '            the following bitmasks are used with the Flags field.
            '            

            Public Const nxFrameFlags_LIN_EventSlot As Byte = &H1
            ' Unconditional frame in event-triggered slot
            ' When Type is nxFrameType_CAN_Data, nxFrameType_CAN_Remote,
            '            nxFrameType_FlexRay_Data, or nxFrameType_LIN_Data,
            '            the following bitmasks are used with the Flags field.
            '            

            Public Const nxFrameFlags_TransmitEcho As Byte = &H80

            ' When Type is nxFrameType_FlexRay_Symbol,
            '            the following values are used with the first Payload byte (offset 0).
            '            

            Public Const nxFlexRaySymbol_MTS As Byte = &H0
            Public Const nxFlexRaySymbol_Wakeup As Byte = &H1

            '
            '            public ushort  nxInternal_PadPayload(ushort payload)
            '            {
            '                return (ushort)(payload) ? (( (ushort)(payload) + 7) & 0x01F8) : 8;
            '            } /* 


            <StructLayout(LayoutKind.Sequential)> _
            Public Structure nxFrameFixed_t
                Public Timestamp As ULong
                Private Identifier As UInteger
                Private Type As Byte
                Private Flags As Byte
                Private Info As Byte
                Private PayloadLength As Byte
                Private Payload As Byte()
                'byte[] Payload = new byte[nxInternal_PadPayload(payld)];
            End Structure

            <StructLayout(LayoutKind.Sequential)> _
            Public Structure nxFrameCAN_t
                Public Timestamp As ULong
                Private Identifier As UInteger
                Private Type As Byte
                Private Flags As Byte
                Private Info As Byte
                Private PayloadLength As Byte
                Private Payload As Byte()
                'byte[] Payload = new byte[((ushort)(8) ? (((ushort)(8) + 7) & 0x01F8) : 8)];
            End Structure

            <StructLayout(LayoutKind.Sequential)> _
            Public Structure nxFrameLIN_t
                Public Timestamp As ULong
                Private Identifier As UInteger
                Private Type As Byte
                Private Flags As Byte
                Private Info As Byte
                Private PayloadLength As Byte
                Private Payload As Byte()
                'byte[] Payload = new byte[((ushort)(8) ? (((ushort)(8) + 7) & 0x01F8) : 8)];
            End Structure

            <StructLayout(LayoutKind.Sequential)> _
            Public Structure nxFrameVar_t
                Public Timestamp As ULong
                Private Identifier As UInteger
                Private Type As Byte
                Private Flags As Byte
                Private Info As Byte
                Private PayloadLength As Byte
                Private Payload As Byte()
                'byte[] Payload = new byte[((ushort)(1) ? (((ushort)(1) + 7) & 0x01F8) : 8)];
            End Structure

            'public nxFrameFixed_type nxFrameFixed_t(ushort payld)
            '            {
            '            }/* 


            '
            '                 struct { \
            '                        nxTimestamp_t       Timestamp; \
            '                        u32                 Identifier; \
            '                        u8                  Type; \
            '                        u8                  Flags; \
            '                        u8                  Info; \
            '                        u8                  PayloadLength; \
            '                        u8                  Payload[ nxInternal_PadPayload(payld) ]; \
            '                     }/* 

            '}

            ' -----------------------------------------------------------------------------
            ' If you are using CVI version 2009 or earlier, you may see a compile error for
            ' this line. Upgrade to CVI version 2010 or later for the fix, disable the
            ' "Build with C99 extensions" compiler option in the "Build Options" dialog of
            ' CVI or edit your copy of the header file to resolve the error.
            ' -----------------------------------------------------------------------------

            'typedef nxFrameFixed_t(8) nxFrameCAN_t;
            'typedef nxFrameFixed_t(8) nxFrameLIN_t;
            'typedef nxFrameFixed_t(1) nxFrameVar_t;

            Public Const nxSizeofFrameHeader As Integer = (16)

            '
            '            public ushort nxFrameSize(ushort payload)
            '            {
            '                return (nxSizeofFrameHeader + nxInternal_PadPayload(payload) );
            '            } /* 


            ' Use this macro to iterate through variable-length frames.
            '            You call this macro as a function, as if it used the following prototype:
            '               nxFrameVar_t * nxFrameIterate(nxFrameVar_t * frameptr);
            '            The input parameter must be initialized to point to the header of a valid frame.
            '            The macro returns a pointer to the header of the next frame in the buffer.
            '            In other words, the macro will iterate from one variable-length frame to
            '            the next variable-length frame.
            '            

            '
            '            unsafe public System.IntPtr nxFrameIterate(byte *frameptr)
            '            {
            '                return (System.IntPtr) ( (byte *)(frameptr) + nxFrameSize((frameptr)->PayloadLength) );
            '            }/* 

        End Class
        ' class xNETConstants
        Private Class PInvoke
            Const niXNETdll As String = "nixnet.dll"
            ' S E S S I O N:  F U N C T I O N S 


            'nxCreateSession (const char * DatabaseName,
            '                               const char * ClusterName,
            '                               const char * List,
            '                               const char * Interface,
            '                               u32 Mode,
            '                               nxSessionRef_t * SessionRef)/*

            <DllImport(niXNETdll, EntryPoint:="nxCreateSession", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function CreateSession(ByVal DatabaseName As String, ByVal ClusterName As String, ByVal List As String, ByVal [Interface] As String, ByVal Mode As UInteger, ByRef sessionRef As System.IntPtr) As Integer
            End Function

            'nxStatus_t _NXFUNC nxCreateSessionByRef (
            '                               u32 NumberOfDatabaseRef,
            '                               nxDatabaseRef_t * ArrayOfDatabaseRef,
            '                               const char * Interface,
            '                               u32 Mode,
            '                               nxSessionRef_t * SessionRef);/* 

            <DllImport(niXNETdll, EntryPoint:="nxCreateSessionByRef", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function createSessionByRef(ByVal NumberOfDatabaseRef As UInteger, ByVal ArrayOfDatabaseRef As UInteger(), ByVal [Interface] As String, ByVal Mode As UInteger, ByRef sessionRef As System.IntPtr) As Integer
            End Function

            'nxStatus_t _NXFUNC nxGetProperty (
            '                                       nxSessionRef_t SessionRef,
            '                                       u32 PropertyID,
            '                                       u32 PropertySize,
            '                                       void * PropertyValue); f64, u32[16], u32[], boolean, double, cstr, nxDatabaseRef_t[] /* 

            <DllImport(niXNETdll, EntryPoint:="nxGetProperty", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_getProperty(ByVal sessionRef As System.IntPtr, ByVal PropertyID As UInteger, ByVal PropertySize As UInteger, ByRef PropertyValue As UInteger) As Integer
            End Function

            <DllImport(niXNETdll, EntryPoint:="nxGetProperty", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_getProperty(ByVal sessionRef As System.IntPtr, ByVal PropertyID As UInteger, ByVal PropertySize As UInteger, ByRef PropertyValue As UInteger()) As Integer
            End Function

            <DllImport(niXNETdll, EntryPoint:="nxGetProperty", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_getProperty(ByVal sessionRef As System.IntPtr, ByVal PropertyID As UInteger, ByVal PropertySize As UInteger, ByRef PropertyValue As Double) As Integer
            End Function

            <DllImport(niXNETdll, EntryPoint:="nxGetProperty", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_getProperty(ByVal sessionRef As System.IntPtr, ByVal PropertyID As UInteger, ByVal PropertySize As UInteger, ByVal PropertyValue As System.Text.StringBuilder) As Integer
            End Function

            'nxStatus_t _NXFUNC nxGetPropertySize (
            '                                      nxSessionRef_t SessionRef,
            '                                      u32 PropertyID,
            '                                      u32 * PropertySize);/* 

            <DllImport(niXNETdll, EntryPoint:="nxGetPropertySize", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_getPropertySize(ByVal sessionRef As System.IntPtr, ByVal PropertyID As UInteger, ByRef PropertySize As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxSetProperty (
            '                                       nxSessionRef_t SessionRef,
            '                                       u32 PropertyID,
            '                                       u32 PropertySize,
            '                                       void * PropertyValue);/* 

            <DllImport(niXNETdll, EntryPoint:="nxSetProperty", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_setProperty(ByVal sessionRef As System.IntPtr, ByVal PropertyID As UInteger, ByVal PropertySize As UInteger, ByVal PropertyValue As IntPtr) As Integer
            End Function

            <DllImport(niXNETdll, EntryPoint:="nxSetProperty", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_setProperty(ByVal sessionRef As System.IntPtr, ByVal PropertyID As UInteger, ByVal PropertySize As UInteger, ByRef PropertyValue As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxGetSubProperty (
            '                                       nxSessionRef_t SessionRef,
            '                                       u32 ActiveIndex,
            '                                       u32 PropertyID,
            '                                       u32 PropertySize,
            '                                       void * PropertyValue);/* 

            '[DllImport(niXNETdll, EntryPoint = "nxGetSubProperty", CallingConvention = CallingConvention.Cdecl)]
            'public static extern unsafe int nx_getSubProperty(System.IntPtr sessionRef, uint ActiveIndex, uint PropertyID, uint PropertySize, void* PropertyValue);

            'nxStatus_t _NXFUNC nxGetSubPropertySize (
            '                                       nxSessionRef_t SessionRef,
            '                                       u32 ActiveIndex,
            '                                       u32 PropertyID,
            '                                       u32 * PropertySize);/* 

            <DllImport(niXNETdll, EntryPoint:="nxGetSubPropertySize", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_getSubPropertySize(ByVal sessionRef As System.IntPtr, ByVal ActiveIndex As UInteger, ByVal PropertyID As UInteger, ByRef PropertySize As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxSetSubProperty (
            '                                       nxSessionRef_t SessionRef,
            '                                       u32 ActiveIndex,
            '                                       u32 PropertyID,
            '                                       u32 PropertySize,
            '                                       void * PropertyValue);/* 

            '[DllImport(niXNETdll, EntryPoint = "nxSetSubProperty", CallingConvention = CallingConvention.Cdecl)]
            'public static extern unsafe int nx_setSubProperty(System.IntPtr sessionRef, uint ActiveIndex, uint PropertyID, uint PropertySize, void* PropertyValue);

            'nxStatus_t _NXFUNC nxReadFrame (
            '                                       nxSessionRef_t SessionRef,
            '                                       void * Buffer,
            '                                       u32 SizeOfBuffer,
            '                                       f64 Timeout,
            '                                       u32 * NumberOfBytesReturned);/* 

            <DllImport(niXNETdll, EntryPoint:="nxReadFrame", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_readFrame(ByVal sessionRef As System.IntPtr, <MarshalAs(UnmanagedType.LPArray)> ByVal Buffer As Byte(), ByVal sizeOfBuffer As UInteger, ByVal timeOut As Double, ByRef NumberOfBytesReturned As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxReadSignalSinglePoint (
            '                                       nxSessionRef_t SessionRef,
            '                                       f64 * ValueBuffer,
            '                                       u32 SizeOfValueBuffer,
            '                                       nxTimestamp_t * TimestampBuffer,
            '                                       u32 SizeOfTimestampBuffer);/* 

            <DllImport(niXNETdll, EntryPoint:="nxReadSignalSinglePoint", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_readSignalSinglePoint(ByVal sessionRef As System.IntPtr, <MarshalAs(UnmanagedType.LPArray)> ByVal ValueBuffer As Double(), ByVal SizeOfValueBuffer As UInteger, <MarshalAs(UnmanagedType.LPArray)> ByVal TimestampBuffer As ULong(), ByVal SizeOfTimestampBuffer As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxReadSignalWaveform (
            '                                       nxSessionRef_t SessionRef,
            '                                       f64 Timeout,
            '                                       nxTimestamp_t * StartTime,
            '                                       f64 * DeltaTime,
            '                                       f64 * ValueBuffer,
            '                                       u32 SizeOfValueBuffer,
            '                                       u32 * NumberOfValuesReturned);/* 

            <DllImport(niXNETdll, EntryPoint:="nxReadSignalWaveform", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_readSignalWaveform(ByVal sessionRef As System.IntPtr, ByVal Timeout As Double, ByRef StartTime As ULong, ByRef DeltaTime As Double, ByVal SizeOfValueBuffer As UInteger, ByRef NumberOfValuesReturned As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxReadSignalXY (
            '                                       nxSessionRef_t SessionRef,
            '                                       nxTimestamp_t * TimeLimit,
            '                                       f64 * ValueBuffer,
            '                                       u32 SizeOfValueBuffer,
            '                                       nxTimestamp_t * TimestampBuffer,
            '                                       u32 SizeOfTimestampBuffer,
            '                                       u32 * NumPairsBuffer,
            '                                       u32 SizeOfNumPairsBuffer);/* 

            <DllImport(niXNETdll, EntryPoint:="nxReadSignalXY", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_readSignalXY(ByVal SessionRef As System.IntPtr, ByVal TimeLimit As ULong, <MarshalAs(UnmanagedType.LPArray)> ByVal ValueBuffer As Double(,), ByVal SizeOfValueBuffer As UInteger, <MarshalAs(UnmanagedType.LPArray)> ByVal TimestampBuffer As ULong(,), ByVal SizeOfTimestampBuffer As UInteger, _
                <MarshalAs(UnmanagedType.LPArray)> ByVal NumPairsBuffer As UInteger(), ByVal SizeOfNumPairsBuffer As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxReadState (
            '                                       nxSessionRef_t SessionRef,
            '                                       u32 StateID,
            '                                       u32 StateSize,
            '                                       void * StateValue,
            '                                       nxStatus_t * Fault);/* 

            <DllImport(niXNETdll, EntryPoint:="nxReadState", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_readState(ByVal sessionRef As System.IntPtr, ByVal StateID As UInteger, ByVal StateSize As UInteger, ByRef StateValue As UInteger, ByRef Fault As Integer) As Integer
            End Function

            <DllImport(niXNETdll, EntryPoint:="nxReadState", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_readState(ByVal sessionRef As System.IntPtr, ByVal StateID As UInteger, ByVal StateSize As UInteger, ByVal StateValue As System.IntPtr, ByRef Fault As Integer) As Integer
            End Function

            'nxStatus_t _NXFUNC nxWriteFrame (
            '                                       nxSessionRef_t SessionRef,
            '                                       void * Buffer,
            '                                       u32 NumberOfBytesForFrames,
            '                                       f64 Timeout);/* 

            <DllImport(niXNETdll, EntryPoint:="nxWriteFrame", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_writeFrame(ByVal sessionRef As System.IntPtr, ByVal buffer As Byte(), ByVal NumberOfBytesForFrames As UInteger, ByVal timeOut As Double) As Integer
            End Function

            'nxStatus_t _NXFUNC nxWriteSignalSinglePoint (
            '                                       nxSessionRef_t SessionRef,
            '                                       f64 * ValueBuffer,
            '                                       u32 SizeOfValueBuffer);/* 

            <DllImport(niXNETdll, EntryPoint:="nxWriteSignalSinglePoint", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_writeSignalSinglePoint(ByVal sessionRef As System.IntPtr, ByVal ValueBuffer As Double(), ByVal SizeOfValueBuffer As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxWriteState (
            '                                       nxSessionRef_t SessionRef,
            '                                       u32 StateID,
            '                                       u32 StateSize,
            '            //                           void * StateValue);/* 

            <DllImport(niXNETdll, EntryPoint:="nxWriteState", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_writeState(ByVal sessionRef As System.IntPtr, ByVal StateID As UInteger, ByVal StateSize As UInteger, ByRef StateValue As System.IntPtr) As Integer
            End Function

            'nxStatus_t _NXFUNC nxWriteSignalWaveform (
            '                                       nxSessionRef_t SessionRef,
            '                                       f64 Timeout,
            '                                       f64 * ValueBuffer,
            '                                       u32 SizeOfValueBuffer);/* 

            <DllImport(niXNETdll, EntryPoint:="nxWriteSignalWaveform", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_writeSignalWaveform(ByVal sessionRef As System.IntPtr, ByVal timeOut As Double, ByVal ValueBuffer As Double(,), ByVal SizeOfValueBuffer As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxWriteSignalXY (
            '                                       nxSessionRef_t SessionRef,
            '                                       f64 Timeout,
            '                                       f64 * ValueBuffer,
            '                                       u32 SizeOfValueBuffer,
            '                                       nxTimestamp_t * TimestampBuffer,
            '                                       u32 SizeOfTimestampBuffer,
            '                                       u32 * NumPairsBuffer,
            '                                       u32 SizeOfNumPairsBuffer);/* 

            <DllImport(niXNETdll, EntryPoint:="nxWriteSignalXY", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_writeSignalXY(ByVal sessionRef As System.IntPtr, ByVal timeOut As Double, ByVal ValueBuffer As Double(,), ByVal SizeOfValueBuffer As UInteger, ByVal TimestampBuffer As ULong(,), ByVal SizeOfTimestampBuffer As UInteger, _
                ByVal NumPairsBuffer As UInteger(), ByVal SizeOfNumPairsBuffer As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxConvertFramesToSignalsSinglePoint (
            '                                       nxSessionRef_t SessionRef,
            '                                       void * FrameBuffer,
            '                                       u32 NumberOfBytesForFrames,
            '                                       f64 * ValueBuffer,
            '                                       u32 SizeOfValueBuffer,
            '                                       nxTimestamp_t * TimestampBuffer,
            '                                       u32 SizeOfTimestampBuffer);/* 

            '[DllImport(niXNETdll, EntryPoint = "nxConvertFramesToSignalsSinglePoint", CallingConvention = CallingConvention.Cdecl)]
            'public static extern unsafe int nx_convertFramesToSignalsSinglePoint(System.IntPtr sessionRef, void* FrameBuffer, uint NumberOfBytesForFrames, out double ValueBuffer, uint SizeOfValueBuffer, out ulong TimestampBuffer, uint SizeOfTimestampBuffer);

            'nxStatus_t _NXFUNC nxConvertSignalsToFramesSinglePoint (
            '                                       nxSessionRef_t SessionRef,
            '                                       f64 * ValueBuffer,
            '                                       u32 SizeOfValueBuffer,
            '                                       void * Buffer,
            '                                       u32 SizeOfBuffer,
            '                                       u32 * NumberOfBytesReturned);/* 

            '[DllImport(niXNETdll, EntryPoint = "nxConvertSignalsToFramesSinglePoint", CallingConvention = CallingConvention.Cdecl)]
            'public static extern unsafe int nx_convertSignalsToFramesSinglePoint(System.IntPtr sessionRef, out double ValueBuffer, uint SizeOfValueBuffer, void* buffer, uint SizeOfBuffer, out uint NumberOfBytesReturned);

            'nxStatus_t _NXFUNC nxBlink (
            '                                       nxSessionRef_t InterfaceRef,
            '                                       u32 Modifier);/* 

            <DllImport(niXNETdll, EntryPoint:="nxBlink", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_blink(ByVal sessionRef As System.IntPtr, ByVal Modifier As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxClear (
            '                                       nxSessionRef_t SessionRef);/* 

            <DllImport(niXNETdll, EntryPoint:="nxClear", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_clear(ByVal sessionRef As System.IntPtr) As Integer
            End Function

            'nxStatus_t _NXFUNC nxConnectTerminals (
            '                                       nxSessionRef_t SessionRef,
            '                                       const char * source,
            '                                       const char * destination);/* 

            <DllImport(niXNETdll, EntryPoint:="nxConnectTerminals", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_connectTerminals(ByVal sessionRef As System.IntPtr, ByVal source As String, ByVal destiniation As String) As Integer
            End Function

            'nxStatus_t _NXFUNC nxDisconnectTerminals (
            '                                       nxSessionRef_t SessionRef,
            '                                       const char * source,
            '                                       const char * destination);/* 

            <DllImport(niXNETdll, EntryPoint:="nxDisconnectTerminals", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_disconnectTerminals(ByVal sessionRef As System.IntPtr, ByVal source As String, ByVal destiniation As String) As Integer
            End Function

            'nxStatus_t _NXFUNC nxFlush (
            '                                       nxSessionRef_t SessionRef);/* 

            <DllImport(niXNETdll, EntryPoint:="nxFlush", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_flush(ByVal sessionRef As System.IntPtr) As Integer
            End Function

            'nxStatus_t _NXFUNC nxStart (
            '                                       nxSessionRef_t SessionRef,
            '                                       u32 Scope);/* 

            <DllImport(niXNETdll, EntryPoint:="nxStart", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_start(ByVal sessionRef As System.IntPtr, ByVal Scope As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxStop (
            '                                       nxSessionRef_t SessionRef,
            '                                       u32 Scope);/* 

            <DllImport(niXNETdll, EntryPoint:="nxStop", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_stop(ByVal sessionRef As System.IntPtr, ByVal Scope As UInteger) As Integer
            End Function

            'void _NXFUNC nxStatusToString (
            '                                       nxStatus_t Status,
            '                                       u32 SizeofString,
            '                                       char * StatusDescription);/* 

            <DllImport(niXNETdll, EntryPoint:="nxStatusToString", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_statusToString(ByVal Status As Integer, ByVal SizeOfString As UInteger, ByVal StatusDescription As System.Text.StringBuilder) As Integer
            End Function

            'nxStatus_t _NXFUNC nxSystemOpen (
            '                                       nxSessionRef_t * SystemRef);/* 

            <DllImport(niXNETdll, EntryPoint:="nxSystemOpen", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_systemOpen(ByRef sessionRef As System.IntPtr) As Integer
            End Function

            'nxStatus_t _NXFUNC nxSystemClose (
            '                                       nxSessionRef_t SystemRef);/* 

            <DllImport(niXNETdll, EntryPoint:="nxSystemClose", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_systemClose(ByVal sessionRef As System.IntPtr) As Integer
            End Function

            'nxStatus_t _NXFUNC nxWait (
            '                                       nxSessionRef_t SessionRef,
            '                                       u32 Condition,
            '                                       u32 ParamIn,
            '                                       f64 Timeout,
            '                                       u32 * ParamOut);/* 

            <DllImport(niXNETdll, EntryPoint:="nxWait", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nx_wait(ByVal sessionRef As System.IntPtr, ByVal Condition As UInteger, ByVal ParamIn As UInteger, ByVal timeOut As Double, ByRef ParamOut As UInteger) As Integer
            End Function

            '************************************************************************************
            '            *                                                                                   *
            '            *                     D A T A B A S E: F U N C T I O N S                            *
            '            *                                                                                   *
            '            ************************************************************************************


            'nxStatus_t _NXFUNC nxdbOpenDatabase (
            '                               const char * DatabaseName,
            '                               nxDatabaseRef_t * DatabaseRef);/* 

            <DllImport(niXNETdll, EntryPoint:="nxdbOpenDatabase", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_openDatabase(ByVal DatabaseName As String, ByRef DatabaseRef As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbCloseDatabase (
            '                               nxDatabaseRef_t DatabaseRef,
            '                               u32 CloseAllRefs); /* 

            <DllImport(niXNETdll, EntryPoint:="nxdbCloseDatabase", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_closeDatabase(ByVal DatabaseRef As UInteger, ByVal CloseAllRefs As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbCreateObject (
            '                               nxDatabaseRef_t ParentObjectRef,
            '                               u32 ObjectClass,
            '                               const char * ObjectName,
            '                               nxDatabaseRef_t * DbObjectRef); /* 

            <DllImport(niXNETdll, EntryPoint:="nxdbCreateObject", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_createObject(ByVal ParentObjectRef As UInteger, ByVal ObjectClass As UInteger, ByVal ObjectName As String, ByRef DbObjectRef As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbFindObject (
            '                               nxDatabaseRef_t ParentObjectRef,
            '                               u32 ObjectClass,
            '                               const char * ObjectName,
            '                               nxDatabaseRef_t * DbObjectRef);/* 

            <DllImport(niXNETdll, EntryPoint:="nxdbFindObject", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_findObject(ByVal ParentObjectRef As UInteger, ByVal ObjectClass As UInteger, ByVal ObjectName As String, ByRef DbObjectRef As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbDeleteObject (
            '                                       nxDatabaseRef_t DbObjectRef);/* 

            <DllImport(niXNETdll, EntryPoint:="nxdbDeleteObject", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_deleteObject(ByVal DbObjectRef As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbSaveDatabase (
            '                                       nxDatabaseRef_t DatabaseRef,
            '                                       const char * DbFilepath); /* 

            <DllImport(niXNETdll, EntryPoint:="nxdbSaveDatabase", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_saveDatabase(ByVal DatabaseRef As UInteger, ByVal DbFilepath As String) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbGetProperty (
            '                                       nxDatabaseRef_t DbObjectRef,
            '                                       u32 PropertyID,
            '                                       u32 PropertySize,
            '                                       void * PropertyValue); f64, u32[16], u32[], boolean, double, cstr, nxDatabaseRef_t[] /* 

            <DllImport(niXNETdll, EntryPoint:="nxdbGetProperty", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_getProperty(ByVal DbObjectRef As UInteger, ByVal PropertyID As UInteger, ByVal PropertySize As UInteger, ByRef PropertyValue As UInteger) As Integer
            End Function

            <DllImport(niXNETdll, EntryPoint:="nxdbGetProperty", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_getProperty(ByVal DbObjectRef As UInteger, ByVal PropertyID As UInteger, ByVal PropertySize As UInteger, ByRef PropertyValue As UInteger()) As Integer
            End Function

            <DllImport(niXNETdll, EntryPoint:="nxdbGetProperty", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_getProperty(ByVal DbObjectRef As UInteger, ByVal PropertyID As UInteger, ByVal PropertySize As UInteger, ByRef PropertyValue As Double) As Integer
            End Function

            <DllImport(niXNETdll, EntryPoint:="nxdbGetProperty", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_getProperty(ByVal DbObjectRef As UInteger, ByVal PropertyID As UInteger, ByVal PropertySize As UInteger, ByVal PropertyValue As System.Text.StringBuilder) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbGetPropertySize (
            '                                       nxDatabaseRef_t DbObjectRef,
            '                                       u32 PropertyID,
            '                                       u32 * PropertySize);/* 

            <DllImport(niXNETdll, EntryPoint:="nxdbGetPropertySize", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_getPropertySize(ByVal DbObjectRef As UInteger, ByVal PropertyID As UInteger, ByRef PropertySize As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbSetProperty (
            '                                       nxDatabaseRef_t DbObjectRef,
            '                                       u32 PropertyID,
            '                                       u32 PropertySize,
            '                                       void * PropertyValue);/* 

            <DllImport(niXNETdll, EntryPoint:="nxdbSetProperty", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_setProperty(ByVal DbObjectRef As UInteger, ByVal PropertyID As UInteger, ByVal PropertySize As UInteger, ByVal PropertyValue As System.IntPtr) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbGetDBCAttributeSize (
            '                           nxDatabaseRef_t DbObjectRef,
            '                           u32 Mode,
            '                           const char* AttributeName,
            '                           u32 *AttributeTextSize);/* 

            <DllImport(niXNETdll, EntryPoint:="nxdbGetDBCAttributeSize", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_getDBCAttributeSize(ByVal DbObjectRef As UInteger, ByVal Mode As UInteger, ByVal AttributeName As String, ByRef AttributeTextSize As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbGetDBCAttribute (
            '                                       nxDatabaseRef_t DbObjectRef,
            '                                       u32 Mode,
            '                                       const char* AttributeName,
            '                                       u32 AttributeTextSize,
            '                                       char* AttributeText,
            '                                       u32 * IsDefault);/* 

            <DllImport(niXNETdll, EntryPoint:="nxdbGetDBCAttribute", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_getDBCAttribute(ByVal DbObjectRef As UInteger, ByVal Mode As UInteger, ByVal AttributeName As String, ByVal AttributeTextSize As UInteger, ByRef AttributeText As String, ByRef IsDefault As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbAddAlias (
            '                                       const char * DatabaseAlias,
            '                                       const char * DatabaseFilepath,
            '                                       u32          DefaultBaudRate);/* 

            <DllImport(niXNETdll, EntryPoint:="nxdbAddAlias", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_addAlias(ByVal DatabaseAlias As String, ByVal DatabaseFilepath As String, ByVal DefaultBaudRate As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbRemoveAlias (
            '                                       const char * DatabaseAlias);/* 

            <DllImport(niXNETdll, EntryPoint:="nxdbRemoveAlias", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_removeAlias(ByVal DatabaseAlias As String) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbDeploy (
            '                                       const char * IPAddress,
            '                                       const char * DatabaseAlias,
            '                                       u32 WaitForComplete,
            '                                       u32 * PercentComplete);/* 

            <DllImport(niXNETdll, EntryPoint:="nxdbDeploy", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_deploy(ByVal IPAddress As String, ByVal DatabaseAlias As String, ByVal WaitForComplete As UInteger, ByRef PercentComplete As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbUndeploy (
            '                                       const char * IPAddress,
            '                                       const char * DatabaseAlias);/* 

            <DllImport(niXNETdll, EntryPoint:="nxdbUndeploy", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_undeploy(ByVal IPAddress As String, ByVal DatabaseAlias As String) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbGetDatabaseList (
            '                                       const char * IPAddress,
            '                                       u32 SizeofAliasBuffer,
            '                                       char * AliasBuffer,
            '                                       u32 SizeofFilepathBuffer,
            '                                       char * FilepathBuffer,
            '                                       u32 * NumberOfDatabases);/* 

            <DllImport(niXNETdll, EntryPoint:="nxdbGetDatabaseList", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_getDatabaseList(ByVal IPAddress As String, ByVal SizeofAliasBuffer As UInteger, ByRef AliasBuffer As String, ByVal SizeofFilepathBuffer As UInteger, ByRef FilepathBuffer As String, ByRef NumberOfDatabases As UInteger) As Integer
            End Function

            'nxStatus_t _NXFUNC nxdbGetDatabaseListSizes (
            '                                       const char * IPAddress,
            '                                       u32 * SizeofAliasBuffer,
            '                                       u32 * SizeofFilepathBuffer);/* 

            <DllImport(niXNETdll, EntryPoint:="nxdbGetDatabaseListSizes", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function nxdb_getDatabaseListSizes(ByVal IPAddress As String, ByRef SizeofAliasBuffer As UInteger, ByRef SizeofFilepathBuffer As UInteger) As Integer
            End Function


            Public Shared Function TestForError(ByVal handle As System.IntPtr, ByVal status As Integer) As Integer
                If (status < 0) Then
                    PInvoke.ThrowError(handle, status)
                End If
                Return status
            End Function

            Public Shared Function ThrowError(ByVal handle As System.IntPtr, ByVal errorCode As Integer) As Integer

                'according to the user guide, 2048 should be enough for the messages
                Dim size As UInteger = 2048


                Dim errorMsg As New System.Text.StringBuilder()
                errorMsg.Capacity = 2048
                'the xnet dll does not have a way to get the size before getting the message
                PInvoke.nx_statusToString(errorCode, size, errorMsg)

                Throw New System.Runtime.InteropServices.ExternalException(errorMsg.ToString(), errorCode)
            End Function

        End Class
        ' PInvoke
    End Class
    ' xnet class

    Public Class XNETHelper
        Inherits Object


        Public Function nx_GetDBAlias(ByRef NumberOfDatabases As UInteger, ByRef AllDBAliases As String()) As Integer
            Dim pInvokeResult As Integer = PInvoke.GetDBAlias(NumberOfDatabases, AllDBAliases)
            'PInvoke.TestForError(Me._handle, pInvokeResult)
            Return pInvokeResult
        End Function

        Private Class PInvoke
            Const helperdll As String = "XNET_Helper.dll"

            'nxExampleGetDBAlias(int * pNumberOfDatabases, char *** pDatabaseAliasArray)

            <DllImport(helperdll, EntryPoint:="nxExampleGetDBAlias", CallingConvention:=CallingConvention.Cdecl)> _
            Public Shared Function GetDBAlias(ByRef NumberOfDatabases As UInteger, ByRef DatabaseAliasArray As String()) As Integer
            End Function

        End Class


    End Class
End Namespace
' namespace

