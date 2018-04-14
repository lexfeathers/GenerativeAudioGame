/**
 * Copyright (c) 2014,2015,2016 Enzien Audio Ltd.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, and/or
 * sublicense copies of the Software, strictly on a non-commercial basis,
 * and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 *
 * DO NOT MODIFY. THIS CODE IS MACHINE GENERATED BY THE ENZIEN AUDIO HEAVY COMPILER.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;
using AOT;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Hv_engine_AudioLib))]
public class Hv_engine_Editor : Editor {

  [MenuItem("Heavy/engine")]
  static void CreateHv_engine() {
    GameObject target = Selection.activeGameObject;
    if (target != null) {
      target.AddComponent<Hv_engine_AudioLib>();
    }
  }
  
  private Hv_engine_AudioLib _dsp;

  private void OnEnable() {
    _dsp = target as Hv_engine_AudioLib;
  }

  public override void OnInspectorGUI() {
    bool isEnabled = _dsp.IsInstantiated();
    if (!isEnabled) {
      EditorGUILayout.LabelField("Press Play!",  EditorStyles.centeredGreyMiniLabel);
    }
    // events
    GUI.enabled = isEnabled;
    EditorGUILayout.Space();
    // masterVoiceOnOff
    if (GUILayout.Button("masterVoiceOnOff")) {
      _dsp.SendEvent(Hv_engine_AudioLib.Event.Mastervoiceonoff);
    }
    
    GUILayout.EndVertical();

    // parameters
    GUI.enabled = true;
    GUILayout.BeginVertical();
    EditorGUILayout.Space();
    EditorGUI.indentLevel++;
    
    // amplitude
    GUILayout.BeginHorizontal();
    float amplitude = _dsp.GetFloatParameter(Hv_engine_AudioLib.Parameter.Amplitude);
    float newAmplitude = EditorGUILayout.Slider("amplitude", amplitude, 0.0f, 0.6f);
    if (amplitude != newAmplitude) {
      _dsp.SetFloatParameter(Hv_engine_AudioLib.Parameter.Amplitude, newAmplitude);
    }
    GUILayout.EndHorizontal();
    
    // filterFreq
    GUILayout.BeginHorizontal();
    float filterFreq = _dsp.GetFloatParameter(Hv_engine_AudioLib.Parameter.Filterfreq);
    float newFilterfreq = EditorGUILayout.Slider("filterFreq", filterFreq, 0.0f, 12000.0f);
    if (filterFreq != newFilterfreq) {
      _dsp.SetFloatParameter(Hv_engine_AudioLib.Parameter.Filterfreq, newFilterfreq);
    }
    GUILayout.EndHorizontal();
    
    // oscFreq
    GUILayout.BeginHorizontal();
    float oscFreq = _dsp.GetFloatParameter(Hv_engine_AudioLib.Parameter.Oscfreq);
    float newOscfreq = EditorGUILayout.Slider("oscFreq", oscFreq, 45.0f, 1200.0f);
    if (oscFreq != newOscfreq) {
      _dsp.SetFloatParameter(Hv_engine_AudioLib.Parameter.Oscfreq, newOscfreq);
    }
    GUILayout.EndHorizontal();
    EditorGUI.indentLevel--;
  }
}
#endif // UNITY_EDITOR

[RequireComponent (typeof (AudioSource))]
public class Hv_engine_AudioLib : MonoBehaviour {
  
  // Events are used to trigger bangs in the patch context (thread-safe).
  // Example usage:
  /*
    void Start () {
        Hv_engine_AudioLib script = GetComponent<Hv_engine_AudioLib>();
        script.SendEvent(Hv_engine_AudioLib.Event.Mastervoiceonoff);
    }
  */
  public enum Event : uint {
    Mastervoiceonoff = 0x82991821,
  }
  
  // Parameters are used to send float messages into the patch context (thread-safe).
  // Example usage:
  /*
    void Start () {
        Hv_engine_AudioLib script = GetComponent<Hv_engine_AudioLib>();
        // Get and set a parameter
        float amplitude = script.GetFloatParameter(Hv_engine_AudioLib.Parameter.Amplitude);
        script.SetFloatParameter(Hv_engine_AudioLib.Parameter.Amplitude, amplitude + 0.1f);
    }
  */
  public enum Parameter : uint {
    Amplitude = 0xE6DE7F45,
    Filterfreq = 0xA53CF95F,
    Oscfreq = 0x204E6574,
  }
  
  // Delegate method for receiving float messages from the patch context (thread-safe).
  // Example usage:
  /*
    void Start () {
        Hv_engine_AudioLib script = GetComponent<Hv_engine_AudioLib>();
        script.RegisterSendHook();
        script.FloatReceivedCallback += OnFloatMessage;
    }

    void OnFloatMessage(Hv_engine_AudioLib.FloatMessage message) {
        Debug.Log(message.receiverName + ": " + message.value);
    }
  */
  public class FloatMessage {
    public string receiverName;
    public float value;

    public FloatMessage(string name, float x) {
      receiverName = name;
      value = x;
    }
  }
  public delegate void FloatMessageReceived(FloatMessage message);
  public FloatMessageReceived FloatReceivedCallback;
  public float amplitude = 0.0f;
  public float filterFreq = 1000.0f;
  public float oscFreq = 80.0f;

  // internal state
  private Hv_engine_Context _context;

  public bool IsInstantiated() {
    return (_context != null);
  }

  public void RegisterSendHook() {
    _context.RegisterSendHook();
  }
  
  // see Hv_engine_AudioLib.Event for definitions
  public void SendEvent(Hv_engine_AudioLib.Event e) {
    if (IsInstantiated()) _context.SendBangToReceiver((uint) e);
  }
  
  // see Hv_engine_AudioLib.Parameter for definitions
  public float GetFloatParameter(Hv_engine_AudioLib.Parameter param) {
    switch (param) {
      case Parameter.Amplitude: return amplitude;
      case Parameter.Filterfreq: return filterFreq;
      case Parameter.Oscfreq: return oscFreq;
      default: return 0.0f;
    }
  }

  public void SetFloatParameter(Hv_engine_AudioLib.Parameter param, float x) {
    switch (param) {
      case Parameter.Amplitude: {
        x = Mathf.Clamp(x, 0.0f, 0.6f);
        amplitude = x;
        break;
      }
      case Parameter.Filterfreq: {
        x = Mathf.Clamp(x, 0.0f, 12000.0f);
        filterFreq = x;
        break;
      }
      case Parameter.Oscfreq: {
        x = Mathf.Clamp(x, 45.0f, 1200.0f);
        oscFreq = x;
        break;
      }
      default: return;
    }
    if (IsInstantiated()) _context.SendFloatToReceiver((uint) param, x);
  }
  
  public void FillTableWithMonoAudioClip(string tableName, AudioClip clip) {
    if (clip.channels > 1) {
      Debug.LogWarning("Hv_engine_AudioLib: Only loading first channel of '" +
          clip.name + "' into table '" +
          tableName + "'. Multi-channel files are not supported.");
    }
    float[] buffer = new float[clip.samples]; // copy only the 1st channel
    clip.GetData(buffer, 0);
    _context.FillTableWithFloatBuffer(tableName, buffer);
  }

  public void FillTableWithFloatBuffer(string tableName, float[] buffer) {
    _context.FillTableWithFloatBuffer(tableName, buffer);
  }

  private void Awake() {
    _context = new Hv_engine_Context((double) AudioSettings.outputSampleRate);
  }
  
  private void Start() {
    _context.SendFloatToReceiver((uint) Parameter.Amplitude, amplitude);
    _context.SendFloatToReceiver((uint) Parameter.Filterfreq, filterFreq);
    _context.SendFloatToReceiver((uint) Parameter.Oscfreq, oscFreq);
  }
  
  private void Update() {
    // retreive sent messages
    if (_context.IsSendHookRegistered()) {
      Hv_engine_AudioLib.FloatMessage tempMessage;
      while ((tempMessage = _context.msgQueue.GetNextMessage()) != null) {
        FloatReceivedCallback(tempMessage);
      }
    }
  }

  private void OnAudioFilterRead(float[] buffer, int numChannels) {
    Assert.AreEqual(numChannels, _context.GetNumOutputChannels()); // invalid channel configuration
    _context.Process(buffer, buffer.Length / numChannels); // process dsp
  }
}

class Hv_engine_Context {

#if UNITY_IOS && !UNITY_EDITOR
  private const string _dllName = "__Internal";
#else
  private const string _dllName = "Hv_engine_AudioLib";
#endif

  // Thread-safe message queue
  public class SendMessageQueue {
    private readonly object _msgQueueSync = new object();
    private readonly Queue<Hv_engine_AudioLib.FloatMessage> _msgQueue = new Queue<Hv_engine_AudioLib.FloatMessage>();

    public Hv_engine_AudioLib.FloatMessage GetNextMessage() {
      lock (_msgQueueSync) {
        return (_msgQueue.Count != 0) ? _msgQueue.Dequeue() : null;
      }
    }

    public void AddMessage(string receiverName, float value) {
      Hv_engine_AudioLib.FloatMessage msg = new Hv_engine_AudioLib.FloatMessage(receiverName, value);
      lock (_msgQueueSync) {
        _msgQueue.Enqueue(msg);
      }
    }
  }

  public readonly SendMessageQueue msgQueue = new SendMessageQueue();
  private readonly GCHandle gch;
  private readonly IntPtr _context; // handle into unmanaged memory
  private SendHook _sendHook = null;

  [DllImport (_dllName)]
  private static extern IntPtr hv_engine_new_with_options(double sampleRate, int poolKb, int queueKb);

  [DllImport (_dllName)]
  private static extern int hv_processInlineInterleaved(IntPtr ctx,
      [In] float[] inBuffer, [Out] float[] outBuffer, int numSamples);

  [DllImport (_dllName)]
  private static extern void hv_delete(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern double hv_getSampleRate(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern int hv_getNumInputChannels(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern int hv_getNumOutputChannels(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern void hv_setSendHook(IntPtr ctx, SendHook sendHook);

  [DllImport (_dllName)]
  private static extern void hv_setPrintHook(IntPtr ctx, PrintHook printHook);

  [DllImport (_dllName)]
  private static extern int hv_setUserData(IntPtr ctx, IntPtr userData);

  [DllImport (_dllName)]
  private static extern IntPtr hv_getUserData(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern void hv_sendBangToReceiver(IntPtr ctx, uint receiverHash);

  [DllImport (_dllName)]
  private static extern void hv_sendFloatToReceiver(IntPtr ctx, uint receiverHash, float x);

  [DllImport (_dllName)]
  private static extern uint hv_msg_getTimestamp(IntPtr message);

  [DllImport (_dllName)]
  private static extern bool hv_msg_hasFormat(IntPtr message, string format);

  [DllImport (_dllName)]
  private static extern float hv_msg_getFloat(IntPtr message, int index);

  [DllImport (_dllName)]
  private static extern bool hv_table_setLength(IntPtr ctx, uint tableHash, uint newSampleLength);

  [DllImport (_dllName)]
  private static extern IntPtr hv_table_getBuffer(IntPtr ctx, uint tableHash);

  [DllImport (_dllName)]
  private static extern float hv_samplesToMilliseconds(IntPtr ctx, uint numSamples);

  [DllImport (_dllName)]
  private static extern uint hv_stringToHash(string s);

  private delegate void PrintHook(IntPtr context, string printName, string str, IntPtr message);

  private delegate void SendHook(IntPtr context, string sendName, uint sendHash, IntPtr message);

  public Hv_engine_Context(double sampleRate, int poolKb=10, int queueKb=2) {
    gch = GCHandle.Alloc(msgQueue);
    _context = hv_engine_new_with_options(sampleRate, poolKb, queueKb);
    hv_setPrintHook(_context, new PrintHook(OnPrint));
    hv_setUserData(_context, GCHandle.ToIntPtr(gch));
  }

  ~Hv_engine_Context() {
    hv_delete(_context);
    GC.KeepAlive(_context);
    GC.KeepAlive(_sendHook);
    gch.Free();
  }

  public void RegisterSendHook() {
    // Note: send hook functionality only applies to messages containing a single float value
    if (_sendHook == null) {
      _sendHook = new SendHook(OnMessageSent);
      hv_setSendHook(_context, _sendHook);
    }
  }

  public bool IsSendHookRegistered() {
    return (_sendHook != null);
  }

  public double GetSampleRate() {
    return hv_getSampleRate(_context);
  }

  public int GetNumInputChannels() {
    return hv_getNumInputChannels(_context);
  }

  public int GetNumOutputChannels() {
    return hv_getNumOutputChannels(_context);
  }

  public void SendBangToReceiver(uint receiverHash) {
    hv_sendBangToReceiver(_context, receiverHash);
  }

  public void SendFloatToReceiver(uint receiverHash, float x) {
    hv_sendFloatToReceiver(_context, receiverHash, x);
  }

  public void FillTableWithFloatBuffer(string tableName, float[] buffer) {
    uint tableHash = hv_stringToHash(tableName);
    if (hv_table_getBuffer(_context, tableHash) != IntPtr.Zero) {
      hv_table_setLength(_context, tableHash, (uint) buffer.Length);
      Marshal.Copy(buffer, 0, hv_table_getBuffer(_context, tableHash), buffer.Length);
    } else {
      Debug.Log(string.Format("Table '{0}' doesn't exist in the patch context.", tableName));
    }
  }

  public uint StringToHash(string s) {
    return hv_stringToHash(s);
  }

  public int Process(float[] buffer, int numFrames) {
    return hv_processInlineInterleaved(_context, buffer, buffer, numFrames);
  }

  [MonoPInvokeCallback(typeof(PrintHook))]
  private static void OnPrint(IntPtr context, string printName, string str, IntPtr message) {
    float timeInSecs = hv_samplesToMilliseconds(context, hv_msg_getTimestamp(message)) / 1000.0f;
    Debug.Log(string.Format("{0} [{1:0.000}]: {2}", printName, timeInSecs, str));
  }

  [MonoPInvokeCallback(typeof(SendHook))]
  private static void OnMessageSent(IntPtr context, string sendName, uint sendHash, IntPtr message) {
    if (hv_msg_hasFormat(message, "f")) {
      SendMessageQueue msgQueue = (SendMessageQueue) GCHandle.FromIntPtr(hv_getUserData(context)).Target;
      msgQueue.AddMessage(sendName, hv_msg_getFloat(message, 0));
    }
  }
}