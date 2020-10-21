using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data instance;

    public enum Languages { english, spanish }
    public Languages language;

    public Material[] avatarMaterials;
    public Material[] sacredIconMaterials;
    public Material motivationMaterial;
    public Material situationMaterial;
    public Material itemMaterial;
    public Material placeMaterial;
    public Material logicMaterial;

    public Glyph[] glyphs;
    public Occupation[] occupations;
    public SyllablePack[] syllablePacks;
    public RoundPreset[] roundPresets;
    public Reaction[] reactions;

    public List<Sprite> deitySprites;

    public GameObject avatarPrefab;
    public GameObject bubblePrefab;
    public GameObject iconPrefab;
    public GameObject sacredIconPrefab;
    public GameObject disappearEffectPrefab;
    public GameObject fadePrefab;
    public GameObject episodePrefab;

    public AudioClip okButtonPressed;
    public AudioClip noButtonPressed;
    public AudioClip iconSelectionClip;
    public AudioClip sacredIconSelectionClip;
    public AudioClip unselectionClip;
    public AudioClip iconSpawnClip;

    public float turnCooldown;
    public float selectionCooldown;
    public float collisionCooldown;
    public float pushStrength;
    [Range(0f, 1f)] public float chanceOfSpeaking;

    public string itemDescriptionEng;
    public string itemDescriptionSpa;
    public string motivationDescriptionEng;
    public string motivationDescriptionSpa;
    public string situationDescriptionEng;
    public string situationDescriptionSpa;
    public string placeDescriptionEng;
    public string placeDescriptionSpa;

    public int splashBuildIndex;
    public int mainBuildIndex;

    public string[] syllablePack;

    private void Awake() 
    {
        instance = this;
        glyphs = Resources.LoadAll<Glyph>("Glyphs");
        occupations = Resources.LoadAll<Occupation>("Occupations");
        syllablePacks = Resources.LoadAll<SyllablePack>("SyllablePacks");
        roundPresets = Resources.LoadAll<RoundPreset>("RoundPresets");
        reactions = Resources.LoadAll<Reaction>("Reactions");
        avatarMaterials = Resources.LoadAll<Material>("AvatarMaterials");
        sacredIconMaterials = Resources.LoadAll<Material>("SacredIconMaterials");
        deitySprites = new List<Sprite>();
        Sprite[] deitySpritesArray = Resources.LoadAll<Sprite>("DeitySprites");
        foreach (Sprite deitySprite in deitySpritesArray) { deitySprites.Add(deitySprite); }
    }

    public Languages Language() { return language; }

    public Material[] AvatarMaterials() { return avatarMaterials; }
    public Material AvatarMaterial() { return avatarMaterials[Random.Range(0, avatarMaterials.Length)]; }
    public Material[] SacredIconMaterials() { return sacredIconMaterials; }
    public Material SacredIconMaterial() { return sacredIconMaterials[Random.Range(0, sacredIconMaterials.Length)]; }
    public Material MotivationMaterial() { return motivationMaterial; }
    public Material SituationMaterial() { return situationMaterial; }
    public Material ItemMaterial() { return itemMaterial; }
    public Material PlaceMaterial() { return placeMaterial; }
    public Material LogicMaterial() { return logicMaterial; }

    public Glyph[] Glyphs() { return glyphs; }
    public Glyph Glyph() { return glyphs[Random.Range(0, glyphs.Length)]; }
    public Occupation[] Occupations() { return occupations; }
    public Occupation Occupation() { return occupations[Random.Range(0, occupations.Length)]; }
    public SyllablePack[] SyllablePacks() { return syllablePacks; }
    public SyllablePack SyllablePack() { return syllablePacks[Random.Range(0, syllablePacks.Length)]; }
    public RoundPreset[] RoundPresets() { return roundPresets; }
    public RoundPreset RoundPreset(int index) { return roundPresets[index];}
    public Reaction[] Reactions() { return reactions; }

    public GameObject AvatarPrefab() { return avatarPrefab; }
    public GameObject IconPrefab() { return iconPrefab; }
    public GameObject SacredIconPrefab() { return sacredIconPrefab; }
    public GameObject BubblePrefab() { return bubblePrefab; }
    public GameObject DisappearEffectPrefab() { return disappearEffectPrefab; }
    public GameObject FadePrefab() { return fadePrefab; }
    public GameObject EpisodePrefab() { return episodePrefab; }

    public AudioClip OkButtonPressed() { return okButtonPressed; }
    public AudioClip NoButtonPressed() { return noButtonPressed; }
    public AudioClip IconSelectionClip() { return iconSelectionClip; }
    public AudioClip SacredIconSelectionClip() { return sacredIconSelectionClip; }
    public AudioClip UnselectionClip() { return unselectionClip; }
    public AudioClip IconSpawnClip() { return iconSpawnClip; }

    public float TurnCooldown() { return turnCooldown; }
    public float SelectionCooldown() { return selectionCooldown; }
    public float PushStrength() { return pushStrength; }
    public float CollisionCooldown() { return collisionCooldown; }
    public float ChanceOfSpeaking() { return chanceOfSpeaking; }

    public string MotivationDescriptionEng() { return motivationDescriptionEng; }
    public string MotivationDescriptionSpa() { return motivationDescriptionSpa; }
    public string SituationDescriptionEng() { return situationDescriptionEng; }
    public string SituationDescriptionSpa() { return situationDescriptionSpa; }
    public string PlaceDescriptionEng() { return placeDescriptionEng; }
    public string PlaceDescriptionSpa() { return placeDescriptionSpa; }

    public int SplashBuildIndex() { return splashBuildIndex; }
    public int MainBuildIndex() { return mainBuildIndex; }

    public Sprite GetDeitySprite()
    {
        Sprite output = null;
        if (deitySprites.Count > 0)
        {
            output = deitySprites[Random.Range(0, deitySprites.Count)];
            deitySprites.Remove(output);
        }
        return output;
    }
    public string GenerateName()
    {
        string output = syllablePack[Random.Range(0, syllablePack.Length)];
        int rnd = Random.Range(1, 3);
        for (int i = 0; i < rnd; i++)
        {
            output += syllablePack[Random.Range(0, syllablePack.Length)].ToLower();
        }
        return output;
    }
}
