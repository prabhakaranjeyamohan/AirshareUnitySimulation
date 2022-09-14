// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: IUX_DAS.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace IUX {

  /// <summary>Holder for reflection information generated from IUX_DAS.proto</summary>
  public static partial class IUXDASReflection {

    #region Descriptor
    /// <summary>File descriptor for IUX_DAS.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static IUXDASReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg1JVVhfREFTLnByb3RvEgNJVVgaFElVWF9zdWJzdHJ1Y3RzLnByb3RvIjIK",
            "EURBU1NpbXVsYXRpb25EYXRhEh0KBmRyb25lcxgBIAMoCzINLklVWC5TaW1E",
            "cm9uZSKDAQoIU2ltRHJvbmUSIAoIbG9jYXRpb24YASABKAsyDi5JVVguTGF0",
            "TG9uQWx0Eh8KBnN0YXR1cxgCIAEoDjIPLklVWC5TdGF0dXNDb2RlEiYKDHRh",
            "cmdldF9jbGFzcxgDIAEoDjIQLklVWC5UYXJnZXRDbGFzcxIMCgR0aW1lGAQg",
            "ASgDQiMKEGNvbS5haXJzaGFyZS5JVVhCB0lVWF9EQVNIAaICA0lVWFAAYgZw",
            "cm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::IUX.IUXSubstructsReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::IUX.DASSimulationData), global::IUX.DASSimulationData.Parser, new[]{ "Drones" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::IUX.SimDrone), global::IUX.SimDrone.Parser, new[]{ "Location", "Status", "TargetClass", "Time" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class DASSimulationData : pb::IMessage<DASSimulationData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<DASSimulationData> _parser = new pb::MessageParser<DASSimulationData>(() => new DASSimulationData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DASSimulationData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::IUX.IUXDASReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DASSimulationData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DASSimulationData(DASSimulationData other) : this() {
      drones_ = other.drones_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DASSimulationData Clone() {
      return new DASSimulationData(this);
    }

    /// <summary>Field number for the "drones" field.</summary>
    public const int DronesFieldNumber = 1;
    private static readonly pb::FieldCodec<global::IUX.SimDrone> _repeated_drones_codec
        = pb::FieldCodec.ForMessage(10, global::IUX.SimDrone.Parser);
    private readonly pbc::RepeatedField<global::IUX.SimDrone> drones_ = new pbc::RepeatedField<global::IUX.SimDrone>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::IUX.SimDrone> Drones {
      get { return drones_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DASSimulationData);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DASSimulationData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!drones_.Equals(other.drones_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= drones_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      drones_.WriteTo(output, _repeated_drones_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      drones_.WriteTo(ref output, _repeated_drones_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += drones_.CalculateSize(_repeated_drones_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DASSimulationData other) {
      if (other == null) {
        return;
      }
      drones_.Add(other.drones_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            drones_.AddEntriesFrom(input, _repeated_drones_codec);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            drones_.AddEntriesFrom(ref input, _repeated_drones_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class SimDrone : pb::IMessage<SimDrone>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<SimDrone> _parser = new pb::MessageParser<SimDrone>(() => new SimDrone());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SimDrone> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::IUX.IUXDASReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SimDrone() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SimDrone(SimDrone other) : this() {
      location_ = other.location_ != null ? other.location_.Clone() : null;
      status_ = other.status_;
      targetClass_ = other.targetClass_;
      time_ = other.time_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SimDrone Clone() {
      return new SimDrone(this);
    }

    /// <summary>Field number for the "location" field.</summary>
    public const int LocationFieldNumber = 1;
    private global::IUX.LatLonAlt location_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::IUX.LatLonAlt Location {
      get { return location_; }
      set {
        location_ = value;
      }
    }

    /// <summary>Field number for the "status" field.</summary>
    public const int StatusFieldNumber = 2;
    private global::IUX.StatusCode status_ = global::IUX.StatusCode.Nostatus;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::IUX.StatusCode Status {
      get { return status_; }
      set {
        status_ = value;
      }
    }

    /// <summary>Field number for the "target_class" field.</summary>
    public const int TargetClassFieldNumber = 3;
    private global::IUX.TargetClass targetClass_ = global::IUX.TargetClass.NoTargetClass;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::IUX.TargetClass TargetClass {
      get { return targetClass_; }
      set {
        targetClass_ = value;
      }
    }

    /// <summary>Field number for the "time" field.</summary>
    public const int TimeFieldNumber = 4;
    private long time_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long Time {
      get { return time_; }
      set {
        time_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SimDrone);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SimDrone other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Location, other.Location)) return false;
      if (Status != other.Status) return false;
      if (TargetClass != other.TargetClass) return false;
      if (Time != other.Time) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (location_ != null) hash ^= Location.GetHashCode();
      if (Status != global::IUX.StatusCode.Nostatus) hash ^= Status.GetHashCode();
      if (TargetClass != global::IUX.TargetClass.NoTargetClass) hash ^= TargetClass.GetHashCode();
      if (Time != 0L) hash ^= Time.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (location_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Location);
      }
      if (Status != global::IUX.StatusCode.Nostatus) {
        output.WriteRawTag(16);
        output.WriteEnum((int) Status);
      }
      if (TargetClass != global::IUX.TargetClass.NoTargetClass) {
        output.WriteRawTag(24);
        output.WriteEnum((int) TargetClass);
      }
      if (Time != 0L) {
        output.WriteRawTag(32);
        output.WriteInt64(Time);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (location_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Location);
      }
      if (Status != global::IUX.StatusCode.Nostatus) {
        output.WriteRawTag(16);
        output.WriteEnum((int) Status);
      }
      if (TargetClass != global::IUX.TargetClass.NoTargetClass) {
        output.WriteRawTag(24);
        output.WriteEnum((int) TargetClass);
      }
      if (Time != 0L) {
        output.WriteRawTag(32);
        output.WriteInt64(Time);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (location_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Location);
      }
      if (Status != global::IUX.StatusCode.Nostatus) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Status);
      }
      if (TargetClass != global::IUX.TargetClass.NoTargetClass) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) TargetClass);
      }
      if (Time != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(Time);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SimDrone other) {
      if (other == null) {
        return;
      }
      if (other.location_ != null) {
        if (location_ == null) {
          Location = new global::IUX.LatLonAlt();
        }
        Location.MergeFrom(other.Location);
      }
      if (other.Status != global::IUX.StatusCode.Nostatus) {
        Status = other.Status;
      }
      if (other.TargetClass != global::IUX.TargetClass.NoTargetClass) {
        TargetClass = other.TargetClass;
      }
      if (other.Time != 0L) {
        Time = other.Time;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (location_ == null) {
              Location = new global::IUX.LatLonAlt();
            }
            input.ReadMessage(Location);
            break;
          }
          case 16: {
            Status = (global::IUX.StatusCode) input.ReadEnum();
            break;
          }
          case 24: {
            TargetClass = (global::IUX.TargetClass) input.ReadEnum();
            break;
          }
          case 32: {
            Time = input.ReadInt64();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            if (location_ == null) {
              Location = new global::IUX.LatLonAlt();
            }
            input.ReadMessage(Location);
            break;
          }
          case 16: {
            Status = (global::IUX.StatusCode) input.ReadEnum();
            break;
          }
          case 24: {
            TargetClass = (global::IUX.TargetClass) input.ReadEnum();
            break;
          }
          case 32: {
            Time = input.ReadInt64();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code