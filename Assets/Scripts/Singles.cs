using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Singles {

    /// <summary>
    /// Game parameters, world climate parameters, player list, turn count.
    /// </summary>
    public static World World;

    /// <summary>
    /// Control of clicks and in game UI
    /// </summary>
    public static PlayerController PlayerController;

    public static UiController UiController;

    /// <summary>
    /// The biome grid
    /// </summary>
    public static HexGrid Grid;

    /// <summary>
    /// Color repository
    /// </summary>
    public static Colors Colors;

    /// <summary>
    /// Reference to player camera
    /// </summary>
    public static CameraControlls CameraControlls;

    /// <summary>
    /// Sprite repository
    /// </summary>
    public static Sprites Sprites;

    /// <summary>
    /// Audio clip repository
    /// </summary>
    public static Audio Audio;

    /// <summary>
    /// Control of background sounds
    /// </summary>
    public static BackgroundAudio BackgroundAudio;

    public static Templates Templates;

    public static WorldStartParamModel WorldStartParamModel;

    public static TutorialPagesData TutorialPagesData;

    public static Cache Cache;

    public static PlayerAction PlayerAction;
}
