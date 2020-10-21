using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage : MonoBehaviour
{
    public enum States { rhapsode, party, end }
    public States state;

    public static Stage instance;

    public Vector2 lowerLeftBound;
    public Vector2 upperRightBound;

    public TextMeshProUGUI playableLabel;
    public TextMeshProUGUI movesLeftLabel;
    public Image glyphImage;
    public AudioSource soundtrackSource;
    public AudioSource[] fxSources;
    public Transform objectsContainer;
    public Transform spotsContainer;
    public Transform travelersContainer;
    public Transform fadesContainer;
    public Transform episodesContainer;
    private List<Transform> spots;
    private Reaction[] reactions;

    public Playable selected;

    public List<Playable> playableObjects;
    public List<Avatar> avatara;
    public List<Icon> icons;
    public List<SacredIcon> sacredIcons;

    public bool isMovementBeingCharged;
    public Vector3 mousePos;
    public int movesLeft;

    public List<Episode> openEpisodes;
    public List<Episode> closedEpisodes;

    private float selectionTimer;
    private float turnTimer;
    private float collisionTimer;

    public RoundPreset currentRoundPreset;
    public int currentLevelIndex;

    // Setup Methods

    public void Awake() { instance = this; }

    public void Start()
    {
        playableObjects = new List<Playable>();
        avatara = new List<Avatar>();
        icons = new List<Icon>();
        sacredIcons = new List<SacredIcon>();
        reactions = Data.instance.Reactions();
        glyphImage.enabled = false;
        LoadRoundPreset(currentLevelIndex);
        GenerateParty();
        Party();
    }

    // Main Loop Methods

    public void Update()
    {
        if (state == States.rhapsode) { InputLoop(); }
        else if (state == States.party)
        {
            turnTimer -= Time.deltaTime;
            if (turnTimer <= 0) { Rhapsode(); }
        }
        if (collisionTimer > 0) { collisionTimer -= Time.deltaTime; }
    }

    // State Methods

    public void Rhapsode()
    {
        //Fade("Rhapsode turn");
        movesLeftLabel.SetText(movesLeft.ToString());
        state = States.rhapsode;

    }
    public void Party()
    {
        //Fade("Party turn");
        movesLeftLabel.SetText(movesLeft.ToString());
        turnTimer = Data.instance.TurnCooldown();
        Unselect();
        currentLevelIndex++;
        if (currentLevelIndex <= currentRoundPreset.LevelPresets().Length - 1) { RunLevelPreset(); }
        state = States.party;
    }
    public void End()
    {
        Fade("End");
        movesLeftLabel.SetText("");
        Unselect();
        state = States.end;
    }

    // Action Methods

    public void GenerateParty()
    {
        ClearParty();
        avatara = new List<Avatar>();
        spots = new List<Transform>();
        foreach (Transform child in spotsContainer) { spots.Add(child); }
        for(int i = 0; i < spots.Count; i++)
        {
            GenerateTraveler(spots[i]);
        }
    }
    public void Fade(string text)
    {
        GameObject newFadeObject = Instantiate(Data.instance.FadePrefab(), fadesContainer);
        Fade newFade = newFadeObject.GetComponent<Fade>();
        newFade.Setup(text);
    }
    public void LoadRoundPreset(int index)
    {
        currentRoundPreset = Data.instance.RoundPreset(index);
        movesLeft = currentRoundPreset.LevelPresets().Length;
    }
    public void RunLevelPreset()
    {
        LevelPreset currentLevelPreset = currentRoundPreset.LevelPresets()[currentLevelIndex];
        for (int i = 0; i < currentLevelPreset.Glyphs().Length; i++)
        {
            AvatarSpeak(currentLevelPreset.Glyphs()[i]);
        }
        if (currentLevelPreset.NewSacredIcon())
        {
            SpawnSacredIcon();
        }
    }

    public void ClearParty() { foreach (Transform child in travelersContainer) { Destroy(child.gameObject); } }
    public void GenerateTraveler(Transform spot)
    {
        Traveler newTraveler = new Traveler();
        newTraveler.Generate();
        GameObject newAvatarObject = Instantiate(Data.instance.AvatarPrefab(), spot.transform.position, spot.rotation, travelersContainer);
        Avatar newAvatar = newAvatarObject.GetComponent<Avatar>();
        newAvatar.Setup(newTraveler);
        avatara.Add(newAvatar);
    }
    public void AvatarSpeak(Avatar avatar)
    {
        Debug.Log(avatar.Traveler().name + " speaks");
        Glyph selectedGlyph = avatar.Traveler().Glyphs()[Random.Range(0, avatar.Traveler().Glyphs().Count)];
        GameObject newBubbleObject = Instantiate(Data.instance.BubblePrefab(), avatar.transform.position + new Vector3(0.75f, 1.5f, 0f), Quaternion.identity, objectsContainer);
        Bubble newBubble = newBubbleObject.GetComponent<Bubble>();
        newBubble.Setup(selectedGlyph);
        SpawnIcon(selectedGlyph);
    }
    public void AvatarSpeak(Glyph selectedGlyph)
    {
        Avatar avatar = avatara[Random.Range(0, avatara.Count)];
        GameObject newBubbleObject = Instantiate(Data.instance.BubblePrefab(), avatar.transform.position + new Vector3(0.75f, 1.5f, 0f), Quaternion.identity, objectsContainer);
        Bubble newBubble = newBubbleObject.GetComponent<Bubble>();
        newBubble.Setup(selectedGlyph);
        SpawnIcon(selectedGlyph);
    }
    public Icon SpawnIcon(Glyph selectedGlyph)
    {
        Icon newIcon;
        Vector3 newIconPos = new Vector3(Random.Range(lowerLeftBound.x, upperRightBound.x), Random.Range(lowerLeftBound.y, upperRightBound.y), 0);
        newIcon = SpawnIcon(selectedGlyph, newIconPos);
        return newIcon;
    }
    public Icon SpawnIcon(Glyph selectedGlyph, Vector3 newIconPos)
    {
        Icon newIcon;
        GameObject newIconObject = Instantiate(Data.instance.IconPrefab(), newIconPos, Quaternion.identity, objectsContainer);
        newIcon = newIconObject.GetComponent<Icon>();
        newIcon.Setup(selectedGlyph);
        icons.Add(newIcon);
        Playable newPlayable = newIconObject.GetComponent<Playable>();
        if (newPlayable != null) { playableObjects.Add(newPlayable); }
        return newIcon;
    }
    public SacredIcon SpawnSacredIcon()
    {
        SacredIcon newSacredIcon;
        Vector3 newIconPos = new Vector3(Random.Range(lowerLeftBound.x, upperRightBound.x), Random.Range(lowerLeftBound.y, upperRightBound.y), 0);
        GameObject newSacredIconObject = Instantiate(Data.instance.SacredIconPrefab(), newIconPos, Quaternion.identity, objectsContainer);
        newSacredIcon = newSacredIconObject.GetComponent<SacredIcon>();
        Deity newDeity = new Deity();
        newDeity.Generate();
        newSacredIcon.Setup(newDeity);
        sacredIcons.Add(newSacredIcon);
        Playable newPlayable = newSacredIconObject.GetComponent<Playable>();
        if (newPlayable != null) { playableObjects.Add(newPlayable); }
        return newSacredIcon;
    }
    public void OnIconCollision(Icon iconA, Icon iconB)
    {
        if (collisionTimer <= 0)
        {
            collisionTimer = Data.instance.CollisionCooldown();
            for (int i = 0; i < reactions.Length; i++)
            {
                List<Glyph> reactionIngredients = reactions[i].Ingredients();
                if (reactionIngredients.Contains(iconA.Glyph()) && reactionIngredients.Contains(iconB.Glyph()))
                {
                    Vector3 newPos = iconA.transform.position + (iconB.transform.position - iconA.transform.position) / 2;
                    iconA.Disappear();
                    iconB.Disappear();
                    SpawnIcon(reactions[i].Result(), newPos);
                    break;
                }
            }
        }
    }
    public void SpawnDisappearEffect(Vector3 pos)
    {
        Instantiate(Data.instance.DisappearEffectPrefab(), pos, Quaternion.identity, objectsContainer);
    }
    public void StartEpisode(SacredIcon sacredIcon, Icon situationIcon)
    {
        GameObject newEpisodeObject = Instantiate(Data.instance.EpisodePrefab(), episodesContainer);
        Episode newEpisode = newEpisodeObject.GetComponent<Episode>();
        newEpisode.Setup(sacredIcon, situationIcon);
        openEpisodes.Add(newEpisode);
    }
    public void CancelEpisode(Episode episode)
    {



    }
    public void CloseEpisode(Episode episode)
    {
        for (int i = 0; i < episode.SacredIcons().Count; i++)
        {
            episode.SacredIcons()[i].Unbind();
        }
        for (int i = 0; i < episode.Icons().Count; i++)
        {
            episode.Icons()[i].Disappear();
        }
    }

    public void Play(AudioClip clip)
    {
        foreach(AudioSource audioSource in fxSources)
        {
            if (!audioSource.isPlaying) 
            {
                audioSource.clip = clip;
                audioSource.Play();
                break;
            }
        }
    }

    // Input Methods

    public void InputLoop()
    {
        if (selectionTimer > 0)
        {
            selectionTimer -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(1)) { Unselect(); }
        if (selectionTimer <= 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButton(0) && selected != null)
            {
                isMovementBeingCharged = true;
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    selected.LineRenderer().SetPosition(1, new Vector3(hit.point.x, hit.point.y, 0));
                }
            }
            else if (selected != null && isMovementBeingCharged && Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                float distance;
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 hitPos = new Vector3(hit.point.x, hit.point.y, 0);
                    distance = Vector3.Distance(selected.transform.position, hitPos);
                    selected.AddForce((hitPos - selected.transform.position).normalized * distance * Data.instance.PushStrength());
                }
                selectionTimer = Data.instance.SelectionCooldown();
                movesLeft--;
                if (movesLeft > 0) { Party(); }
                else { End(); }
            }
            else { isMovementBeingCharged = false; }
        }
    }
    public void Clicked(Playable playable)
    {
        if (state == States.rhapsode && selected == null)
        {
            selectionTimer = Data.instance.SelectionCooldown();
            selected = playable;
            playable.Selected();
        }
    }
    public void DestroyIcon(Icon icon)
    {
        Playable ply = icon.gameObject.GetComponent<Playable>();
        if (ply != null && selected == ply) { Unselect(); }
        if (playableObjects.Contains(ply)) { playableObjects.Remove(ply); }
        if (icons.Contains(icon)) { icons.Remove(icon); }
        Destroy(icon.gameObject);
    }
    public void DestroySacredIcon(SacredIcon icon)
    {
        Playable ply = icon.gameObject.GetComponent<Playable>();
        if (ply != null && selected == ply) { Unselect(); }
        if (playableObjects.Contains(ply)) { playableObjects.Remove(ply); }
        if (sacredIcons.Contains(icon)) { sacredIcons.Remove(icon); }
        Destroy(icon.gameObject);
    }


    public void MouseOver(Avatar avatar)
    {
        if (selected == null)
        {
            if (Data.instance.Language() == Data.Languages.english) { playableLabel.SetText(avatar.name + "\n" + avatar.Traveler().Occupation().NameEng()); }
            else if (Data.instance.Language() == Data.Languages.spanish) { playableLabel.SetText(avatar.name + "\n" + avatar.Traveler().Occupation().NameSpa()); }
        }
    }
    public void MouseOver(Icon icon)
    {
        Glyph glyph = icon.Glyph();
        glyphImage.enabled = true;
        string glyphCategory = "";
        if (selected == null)
        {
            glyphImage.sprite = icon.Glyph().Sprite();
            if (Data.instance.Language() == Data.Languages.english) 
            {
                if (glyph.Category() == Glyph.Categories.motivation) { glyphCategory = Data.instance.MotivationDescriptionEng(); }
                else if (glyph.Category() == Glyph.Categories.situation) { glyphCategory = Data.instance.SituationDescriptionEng(); }
                else if (glyph.Category() == Glyph.Categories.place) { glyphCategory = Data.instance.PlaceDescriptionEng(); }
                playableLabel.SetText(glyph.NameEng() + "\n" + glyphCategory); 
            }
            else if (Data.instance.Language() == Data.Languages.spanish) 
            {
                if (glyph.Category() == Glyph.Categories.motivation) { glyphCategory = Data.instance.MotivationDescriptionSpa(); }
                else if (glyph.Category() == Glyph.Categories.situation) { glyphCategory = Data.instance.SituationDescriptionSpa(); }
                else if (glyph.Category() == Glyph.Categories.place) { glyphCategory = Data.instance.PlaceDescriptionSpa(); }
                playableLabel.SetText(glyph.NameSpa() + "\n" + glyphCategory); 
            }
        }
    }
    public void MouseOver(SacredIcon sacredIcon)
    {

        Deity deity = sacredIcon.Deity();
        if (selected == null)
        {
            glyphImage.enabled = true;
            glyphImage.sprite = deity.Sprite();
            playableLabel.SetText(deity.Name());
        }
    }
    public void MouseExit() 
    {
        if (selected == null) 
        {
            glyphImage.sprite = null;
            glyphImage.enabled = false;
            playableLabel.SetText(""); 
        } 
    }

    public void Unselect()
    {
        glyphImage.sprite = null;
        glyphImage.enabled = false;
        playableLabel.SetText("");
        selected = null;
        foreach (Playable playable in playableObjects) { playable.Unselected(); }
        foreach (Icon icon in icons) { icon.Unselected(); }
        foreach (SacredIcon sacredIcon in sacredIcons) { sacredIcon.Unselected(); }
    }
    public void Unplayable(GameObject go)
    {
        Playable ps = go.GetComponent<Playable>();
        if (ps != null)
        {
            if (playableObjects.Contains(ps))
            {
                playableObjects.Remove(ps);
                Destroy(ps);
            }
        } 
    }
    public void Playable(GameObject go)
    {
        Debug.Log("Making " + go + " playable");
        Playable newPlayableScript = go.AddComponent<Playable>();
        playableObjects.Add(newPlayableScript);
    }

    // Get Set Methods

    public Transform SpotsContainer() { return spotsContainer; }
    public Transform TravelersContainer() { return travelersContainer; }

    public Playable Selected() { return selected; }

    public List<Episode> OpenEpisodes() { return openEpisodes; }
    public List<Episode> ClosedEpisodes() { return closedEpisodes; }

    public States State() { return state; }
    public bool IsMovementBeingCharged() { return isMovementBeingCharged; }
    public int MovesLeft() { return movesLeft; }

}
