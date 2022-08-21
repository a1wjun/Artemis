﻿using System.Collections.Generic;
using RGB.NET.Core;

namespace Artemis.Core.Services;

/// <summary>
///     Utilities for mapping keys and buttons to LEDs
/// </summary>
public static class InputKeyUtilities
{
    /// <summary>
    ///     A dictionary of mappings between <see cref="KeyboardKey" /> and <see cref="LedId" />
    /// </summary>
    public static readonly Dictionary<KeyboardKey, LedId> KeyboardKeyLedIdMap = new()
    {
        {KeyboardKey.None, LedId.Keyboard_Custom1},
        {KeyboardKey.Cancel, LedId.Keyboard_Custom2},
        {KeyboardKey.Back, LedId.Keyboard_Backspace},
        {KeyboardKey.Tab, LedId.Keyboard_Tab},
        {KeyboardKey.LineFeed, LedId.Keyboard_Custom3},
        {KeyboardKey.Clear, LedId.Keyboard_Custom4},
        {KeyboardKey.Enter, LedId.Keyboard_Enter},
        {KeyboardKey.Pause, LedId.Keyboard_PauseBreak},
        {KeyboardKey.CapsLock, LedId.Keyboard_CapsLock},
        {KeyboardKey.HangulMode, LedId.Keyboard_Custom4},
        {KeyboardKey.JunjaMode, LedId.Keyboard_Custom5},
        {KeyboardKey.FinalMode, LedId.Keyboard_Custom6},
        {KeyboardKey.HanjaMode, LedId.Keyboard_Custom7},
        {KeyboardKey.Escape, LedId.Keyboard_Escape},
        {KeyboardKey.ImeConvert, LedId.Keyboard_Custom8},
        {KeyboardKey.ImeNonConvert, LedId.Keyboard_Custom9},
        {KeyboardKey.ImeAccept, LedId.Keyboard_Custom10},
        {KeyboardKey.ImeModeChange, LedId.Keyboard_Custom11},
        {KeyboardKey.Space, LedId.Keyboard_Space},
        {KeyboardKey.PageUp, LedId.Keyboard_PageUp},
        {KeyboardKey.PageDown, LedId.Keyboard_PageDown},
        {KeyboardKey.End, LedId.Keyboard_End},
        {KeyboardKey.Home, LedId.Keyboard_Home},
        {KeyboardKey.Left, LedId.Keyboard_ArrowLeft},
        {KeyboardKey.Up, LedId.Keyboard_ArrowUp},
        {KeyboardKey.Right, LedId.Keyboard_ArrowRight},
        {KeyboardKey.Down, LedId.Keyboard_ArrowDown},
        {KeyboardKey.Select, LedId.Keyboard_Custom12},
        {KeyboardKey.Print, LedId.Keyboard_Custom13},
        {KeyboardKey.Execute, LedId.Keyboard_Custom14},
        {KeyboardKey.PrintScreen, LedId.Keyboard_PrintScreen},
        {KeyboardKey.Insert, LedId.Keyboard_Insert},
        {KeyboardKey.Delete, LedId.Keyboard_Delete},
        {KeyboardKey.Help, LedId.Keyboard_Custom15},
        {KeyboardKey.D0, LedId.Keyboard_0},
        {KeyboardKey.D1, LedId.Keyboard_1},
        {KeyboardKey.D2, LedId.Keyboard_2},
        {KeyboardKey.D3, LedId.Keyboard_3},
        {KeyboardKey.D4, LedId.Keyboard_4},
        {KeyboardKey.D5, LedId.Keyboard_5},
        {KeyboardKey.D6, LedId.Keyboard_6},
        {KeyboardKey.D7, LedId.Keyboard_7},
        {KeyboardKey.D8, LedId.Keyboard_8},
        {KeyboardKey.D9, LedId.Keyboard_9},
        {KeyboardKey.A, LedId.Keyboard_A},
        {KeyboardKey.B, LedId.Keyboard_B},
        {KeyboardKey.C, LedId.Keyboard_C},
        {KeyboardKey.D, LedId.Keyboard_D},
        {KeyboardKey.E, LedId.Keyboard_E},
        {KeyboardKey.F, LedId.Keyboard_F},
        {KeyboardKey.G, LedId.Keyboard_G},
        {KeyboardKey.H, LedId.Keyboard_H},
        {KeyboardKey.I, LedId.Keyboard_I},
        {KeyboardKey.J, LedId.Keyboard_J},
        {KeyboardKey.K, LedId.Keyboard_K},
        {KeyboardKey.L, LedId.Keyboard_L},
        {KeyboardKey.M, LedId.Keyboard_M},
        {KeyboardKey.N, LedId.Keyboard_N},
        {KeyboardKey.O, LedId.Keyboard_O},
        {KeyboardKey.P, LedId.Keyboard_P},
        {KeyboardKey.Q, LedId.Keyboard_Q},
        {KeyboardKey.R, LedId.Keyboard_R},
        {KeyboardKey.S, LedId.Keyboard_S},
        {KeyboardKey.T, LedId.Keyboard_T},
        {KeyboardKey.U, LedId.Keyboard_U},
        {KeyboardKey.V, LedId.Keyboard_V},
        {KeyboardKey.W, LedId.Keyboard_W},
        {KeyboardKey.X, LedId.Keyboard_X},
        {KeyboardKey.Y, LedId.Keyboard_Y},
        {KeyboardKey.Z, LedId.Keyboard_Z},
        {KeyboardKey.LWin, LedId.Keyboard_LeftGui},
        {KeyboardKey.RWin, LedId.Keyboard_RightGui},
        {KeyboardKey.Apps, LedId.Keyboard_Application},
        {KeyboardKey.Sleep, LedId.Keyboard_Custom16},
        {KeyboardKey.NumPad0, LedId.Keyboard_Num0},
        {KeyboardKey.NumPad1, LedId.Keyboard_Num1},
        {KeyboardKey.NumPad2, LedId.Keyboard_Num2},
        {KeyboardKey.NumPad3, LedId.Keyboard_Num3},
        {KeyboardKey.NumPad4, LedId.Keyboard_Num4},
        {KeyboardKey.NumPad5, LedId.Keyboard_Num5},
        {KeyboardKey.NumPad6, LedId.Keyboard_Num6},
        {KeyboardKey.NumPad7, LedId.Keyboard_Num7},
        {KeyboardKey.NumPad8, LedId.Keyboard_Num8},
        {KeyboardKey.NumPad9, LedId.Keyboard_Num9},
        {KeyboardKey.Multiply, LedId.Keyboard_NumAsterisk},
        {KeyboardKey.Add, LedId.Keyboard_NumPlus},
        {KeyboardKey.Separator, LedId.Keyboard_NumEnter}, // unverified
        {KeyboardKey.Subtract, LedId.Keyboard_NumMinus},
        {KeyboardKey.Decimal, LedId.Keyboard_NumPeriodAndDelete},
        {KeyboardKey.Divide, LedId.Keyboard_NumSlash},
        {KeyboardKey.F1, LedId.Keyboard_F1},
        {KeyboardKey.F2, LedId.Keyboard_F2},
        {KeyboardKey.F3, LedId.Keyboard_F3},
        {KeyboardKey.F4, LedId.Keyboard_F4},
        {KeyboardKey.F5, LedId.Keyboard_F5},
        {KeyboardKey.F6, LedId.Keyboard_F6},
        {KeyboardKey.F7, LedId.Keyboard_F7},
        {KeyboardKey.F8, LedId.Keyboard_F8},
        {KeyboardKey.F9, LedId.Keyboard_F9},
        {KeyboardKey.F10, LedId.Keyboard_F10},
        {KeyboardKey.F11, LedId.Keyboard_F11},
        {KeyboardKey.F12, LedId.Keyboard_F12},
        {KeyboardKey.F13, LedId.Keyboard_Custom17},
        {KeyboardKey.F14, LedId.Keyboard_Custom18},
        {KeyboardKey.F15, LedId.Keyboard_Custom19},
        {KeyboardKey.F16, LedId.Keyboard_Custom20},
        {KeyboardKey.F17, LedId.Keyboard_Custom21},
        {KeyboardKey.F18, LedId.Keyboard_Custom22},
        {KeyboardKey.F19, LedId.Keyboard_Custom23},
        {KeyboardKey.F20, LedId.Keyboard_Custom24},
        {KeyboardKey.F21, LedId.Keyboard_Custom25},
        {KeyboardKey.F22, LedId.Keyboard_Custom26},
        {KeyboardKey.F23, LedId.Keyboard_Custom27},
        {KeyboardKey.F24, LedId.Keyboard_Custom28},
        {KeyboardKey.NumLock, LedId.Keyboard_NumLock},
        {KeyboardKey.Scroll, LedId.Keyboard_ScrollLock},
        {KeyboardKey.LeftShift, LedId.Keyboard_LeftShift},
        {KeyboardKey.RightShift, LedId.Keyboard_RightShift},
        {KeyboardKey.LeftCtrl, LedId.Keyboard_LeftCtrl},
        {KeyboardKey.RightCtrl, LedId.Keyboard_RightCtrl},
        {KeyboardKey.LeftAlt, LedId.Keyboard_LeftAlt},
        {KeyboardKey.RightAlt, LedId.Keyboard_RightAlt},
        {KeyboardKey.BrowserBack, LedId.Keyboard_Custom29},
        {KeyboardKey.BrowserForward, LedId.Keyboard_Custom30},
        {KeyboardKey.BrowserRefresh, LedId.Keyboard_Custom31},
        {KeyboardKey.BrowserStop, LedId.Keyboard_Custom32},
        {KeyboardKey.BrowserSearch, LedId.Keyboard_Custom33},
        {KeyboardKey.BrowserFavorites, LedId.Keyboard_Custom34},
        {KeyboardKey.BrowserHome, LedId.Keyboard_Custom35},
        {KeyboardKey.VolumeMute, LedId.Keyboard_MediaMute},
        {KeyboardKey.VolumeDown, LedId.Keyboard_MediaVolumeDown},
        {KeyboardKey.VolumeUp, LedId.Keyboard_MediaVolumeUp},
        {KeyboardKey.MediaNextTrack, LedId.Keyboard_MediaNextTrack},
        {KeyboardKey.MediaPreviousTrack, LedId.Keyboard_MediaPreviousTrack},
        {KeyboardKey.MediaStop, LedId.Keyboard_MediaStop},
        {KeyboardKey.MediaPlayPause, LedId.Keyboard_MediaPlay},
        {KeyboardKey.LaunchMail, LedId.Keyboard_Custom36},
        {KeyboardKey.SelectMedia, LedId.Keyboard_Custom37},
        {KeyboardKey.LaunchApplication1, LedId.Keyboard_Custom38},
        {KeyboardKey.LaunchApplication2, LedId.Keyboard_Custom39},
        {KeyboardKey.OemSemicolon, LedId.Keyboard_SemicolonAndColon},
        {KeyboardKey.OemPlus, LedId.Keyboard_EqualsAndPlus},
        {KeyboardKey.OemMinus, LedId.Keyboard_MinusAndUnderscore},
        {KeyboardKey.OemComma, LedId.Keyboard_CommaAndLessThan},
        {KeyboardKey.OemPeriod, LedId.Keyboard_PeriodAndBiggerThan},
        {KeyboardKey.OemQuestion, LedId.Keyboard_SlashAndQuestionMark},
        {KeyboardKey.OemTilde, LedId.Keyboard_GraveAccentAndTilde},
        {KeyboardKey.AbntC1, LedId.Keyboard_Custom40},
        {KeyboardKey.AbntC2, LedId.Keyboard_Custom41},
        {KeyboardKey.OemOpenBrackets, LedId.Keyboard_BracketLeft},
        {KeyboardKey.OemPipe, LedId.Keyboard_Backslash},
        {KeyboardKey.OemCloseBrackets, LedId.Keyboard_BracketRight},
        {KeyboardKey.OemQuotes, LedId.Keyboard_ApostropheAndDoubleQuote},
        {KeyboardKey.OemBackslash, LedId.Keyboard_Custom42}, // unverified
        {KeyboardKey.ImeProcessed, LedId.Keyboard_Custom43},
        {KeyboardKey.System, LedId.Keyboard_Custom44},
        {KeyboardKey.OemAttn, LedId.Keyboard_Custom45},
        {KeyboardKey.OemFinish, LedId.Keyboard_Custom46},
        {KeyboardKey.OemCopy, LedId.Keyboard_Custom47},
        {KeyboardKey.OemAuto, LedId.Keyboard_Custom48},
        {KeyboardKey.OemEnlw, LedId.Keyboard_Custom49},
        {KeyboardKey.OemBackTab, LedId.Keyboard_Custom50},
        {KeyboardKey.Attn, LedId.Keyboard_Custom51},
        {KeyboardKey.CrSel, LedId.Keyboard_Custom52},
        {KeyboardKey.ExSel, LedId.Keyboard_Custom53},
        {KeyboardKey.EraseEof, LedId.Keyboard_Custom54},
        {KeyboardKey.Play, LedId.Keyboard_MediaPlay},
        {KeyboardKey.Zoom, LedId.Keyboard_Custom55},
        {KeyboardKey.NoName, LedId.Keyboard_Custom56},
        {KeyboardKey.Pa1, LedId.Keyboard_Custom57},
        {KeyboardKey.OemClear, LedId.Keyboard_Custom58},
        {KeyboardKey.DeadCharProcessed, LedId.Keyboard_Custom59},
        {KeyboardKey.NumPadEnter, LedId.Keyboard_NumEnter}
    };

    /// <summary>
    ///     A dictionary of mappings between <see cref="MouseButton" /> and <see cref="LedId" />
    /// </summary>
    public static readonly Dictionary<MouseButton, LedId> MouseButtonLedIdMap = new()
    {
        {MouseButton.Left, LedId.Mouse1},
        {MouseButton.Middle, LedId.Mouse2},
        {MouseButton.Right, LedId.Mouse3},
        {MouseButton.Button4, LedId.Mouse4},
        {MouseButton.Button5, LedId.Mouse5}
    };
}