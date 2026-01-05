# Art Asset Creation & Delivery Guide
## Table of Contents
1. [Overview](#overview)
2. [Tools You'll Need](#tools-youll-need)
3. [Folder Structure](#folder-structure)
   - [Working Files (`art-source/`)](#working-files-art-source)
   - [Export Files (`art-export/`)](#export-files-art-export)
4. [Naming Conventions](#naming-conventions)
   - [3D Models](#3d-models)
   - [Textures](#textures)
   - [UI Sprites & Icons](#ui-sprites--icons)
   - [VFX Textures](#vfx-textures)
5. [Blender to Unity Export Settings](#blender-to-unity-export-settings)
   - [Before You Export](#before-you-export)
   - [FBX Export Settings](#fbx-export-settings)
   - [Creating an Export Preset](#creating-an-export-preset)
6. [Workflow: Creating a New Asset](#workflow-creating-a-new-asset)
   - [Step 1: Check Current Tasks](#step-1-check-current-tasks)
   - [Step 2: Create Branch in Anchorpoint](#step-2-create-branch-in-anchorpoint)
   - [Step 3: Create Your Folders](#step-3-create-your-folders)
   - [Step 4: Work on Your Art](#step-4-work-on-your-art)
   - [Step 5: Export Game-Ready Assets](#step-5-export-game-ready-assets)
   - [Step 6: Commit in Anchorpoint](#step-6-commit-in-anchorpoint)
   - [Step 7: Push & Create Pull Request](#step-7-push--create-pull-request)
   - [Step 8: Feedback & Iteration](#step-8-feedback--iteration)
   - [Step 9: Merge & Cleanup](#step-9-merge--cleanup)
6. [Common Workflows](#common-workflows)
   - [Multiple Assets in One Branch](#multiple-assets-in-one-branch)
   - [Updating an Existing Asset](#updating-an-existing-asset)
   - [Creating Variants](#creating-variants)
7. [Quality Checklist](#quality-checklist)
8. [Troubleshooting](#troubleshooting)
9. [Best Practices](#best-practices)
10. [Scale Reference](#scale-reference)
11. [Getting Help](#getting-help)
12. [Quick Reference Card](#quick-reference-card)
---  
## Overview

This document explains how to create, export, and deliver art assets for our horror game project. You'll work in your preferred tools (Blender, Maya, Photoshop, etc.) and deliver game-ready assets through Anchorpoint (our Git interface).

**You will never need to open Unity.** Your job is to create amazing art and export it properly. The programmer will handle implementation.

---

## Tools You'll Need

- **Your 3D/2D software** (Blender, Maya, Substance Painter, Photoshop, etc.)
- **Anchorpoint** (Git client for artists - no command line needed)
- Access to the project repository

---

## Folder Structure

### Working Files (`art-source/`)

This is where YOU work. Store all your native files here organized by feature/object:

```
art-source/
├── characters/
│   ├── player/
│   ├── monster-asylum/
│   └── ghost-child/
├── environments/
│   ├── asylum-hallway/
│   ├── patient-room/
│   └── surgery-room/
├── props/
│   ├── wheelchair/
│   ├── gurney/
│   └── flashlight/
├── nature/
│   ├── dead-tree-01/
│   └── boulder-set/
├── ui/
│   ├── health-system/
│   ├── inventory/
│   └── main-menu/
└── vfx/
    ├── blood-effects/
    └── fog-system/
```

**Example: Creating a wheelchair prop**
```
art-source/props/wheelchair/
├── wheelchair-model.blend
├── wheelchair-textures.spp
└── wheelchair-icon.psd
```

### Export Files (`art-export/`)

This is where you export game-ready assets. Mirror the same folder structure:

```
art-export/props/wheelchair/
├── SM_Wheelchair_01.fbx
├── T_Wheelchair_BaseColor.png
├── T_Wheelchair_Normal.png
├── T_Wheelchair_Roughness.png
└── UI_Icon_Wheelchair.png
```

---

## Naming Conventions

**CRITICAL:** File names must follow these rules exactly. Git will reject commits with incorrect names.

### 3D Models

| Type | Prefix | Example | When to Use |
|------|--------|---------|-------------|
| Static Mesh | `SM_` | `SM_Wheelchair_01.fbx` | Non-animated objects |
| Skeletal Mesh | `SK_` | `SK_MonsterAsylum_01.fbx` | Rigged characters/objects |
| Animation | `AM_` | `AM_Player_Walk.fbx` | Animation files |

**Format:**
- `SM_ObjectName_01.fbx`
- `SK_ObjectName_01.fbx`
- `AM_ObjectName_ActionName.fbx`

**Rules:**
- Use PascalCase (FirstLetterCapitalized)
- Include 2-digit version number for models
- Use descriptive action names for animations
- Always `.fbx` format

**Examples:**
- ✅ `SM_Gurney_01.fbx`
- ✅ `SK_Player_01.fbx`
- ✅ `AM_MonsterAsylum_Attack.fbx`
- ❌ `wheelchair.fbx` (missing prefix)
- ❌ `SM_wheelchair_01.fbx` (lowercase object name)
- ❌ `SM_Gurney.fbx` (missing version number)

### Textures

**Format:** `T_ObjectName_TextureType.png`

**Texture Types:**
- `BaseColor` - Diffuse/albedo color
- `Normal` - Normal map
- `Roughness` - Roughness map
- `Metallic` - Metallic map
- `AO` - Ambient occlusion
- `Emissive` - Emissive/glow map

**Examples:**
- ✅ `T_Wheelchair_BaseColor.png`
- ✅ `T_MonsterAsylum_Normal.png`
- ✅ `T_HallwayWalls_Roughness.png`
- ❌ `wheelchair_diffuse.png` (wrong format)
- ❌ `T_wheelchair_BaseColor.png` (lowercase object name)

### UI Sprites & Icons

**Format:** `UI_ElementName_State.png`

**Examples:**
- ✅ `UI_HealthBar_Full.png`
- ✅ `UI_HealthBar_Empty.png`
- ✅ `UI_Icon_Flashlight.png`
- ✅ `UI_Button_Start_Normal.png`
- ✅ `UI_Button_Start_Hover.png`
- ✅ `UI_Panel_Inventory.png`

### VFX Textures

**Particles:** `T_ParticleName.png`
- ✅ `T_Blood_Particle.png`
- ✅ `T_Fog_Wispy.png`

**Decals:** `T_Decal_Type_Variant.png`
- ✅ `T_Decal_BloodSplatter_01.png`
- ✅ `T_Decal_Scratch_Wall_01.png`

---

## Blender to Unity Export Settings

**Important:** Blender and Unity use different coordinate systems. Blender uses Z-Up (right-hand), while Unity uses Y-Up (left-hand). Following these settings ensures your models import with correct default values in Unity.

### Before You Export

**1. Check Units**
- Go to Scene Properties (right panel)
- Verify "Unit System" is set to **Metric**
- Unity treats 1 unit = 1 meter

**2. Apply Rotation and Scale**
- Select your object in Object Mode
- Go to **Object > Apply > Rotation & Scale** (or press `Ctrl+A`)
- This sets the default values for position, rotation, and scale
- **Do this EVERY time before exporting**

**3. Set Origin Point**
- The orange dot is your model's origin/pivot point
- Go to **Object > Set Origin** to adjust if needed
- **Recommended origin positions:**
  - **Props:** Bottom-center (sits on ground)
  - **Hanging objects:** Top-center (chandeliers, chains)
  - **Doors:** Edge where hinge is located
  - **Characters:** Between feet at ground level
  - **Symmetrical objects:** Center

### FBX Export Settings

When exporting to FBX, use these settings:

**Main Tab:**
- Select **File > Export > FBX (.fbx)**
- Choose your destination in `art-export/`

**Include Section:**
- ✅ **Selected Objects** (if exporting specific items)
- ✅ **Mesh**
- ✅ **Armature** (for rigged characters only)
- ❌ **Camera, Lamp** (uncheck these)

**Transform Section:**
- **Scale:** FBX Unit Scale
- **Forward:** -Z Forward
- **Up:** Y Up
- ✅ **Apply Unit** (check this)
- ✅ **Apply Transform** (check this)

**Geometry Section:**
- ✅ **Apply Modifiers** (check this)
- **Smoothing:** Face (default is fine)

**Armature Section (for rigged models only):**
- ❌ **Add Leaf Bones** (uncheck this)

**Bake Animation Section (for animations only):**
- ✅ **Bake Animation** (when exporting animations)
- Set **Start** and **End** frames to your animation range

**Path Mode:**
- Set to **Copy**
- ✅ Check **Embed Textures** (if you want textures packed with model)
- Note: We typically export textures separately, so you can uncheck this

<img width="719" height="719" alt="image" src="https://github.com/user-attachments/assets/724ce4cf-60c8-45c6-ad25-c4e84e12eff7" />

### Creating an Export Preset

Save time by creating a preset with these settings:

1. Configure all the settings above
2. At the bottom of the Export window, click the **"+"** button next to "Operator Presets"
3. Name it: **"Unity Export"**
4. Click **OK**

**Next time you export:**
1. Go to File > Export > FBX
2. Click the **"∨"** dropdown next to Operator Presets
3. Select **"Unity Export"**
4. All settings are applied automatically!

### Visual Guide

```
BEFORE EXPORT CHECKLIST:
☐ Units set to Metric
☐ Applied Rotation & Scale (Ctrl+A)
☐ Origin point in correct position
☐ No unnecessary objects in scene

FBX EXPORT QUICK SETTINGS:
Transform:
  Scale: FBX Unit Scale
  Forward: -Z Forward
  Up: Y Up
  ✅ Apply Unit
  ✅ Apply Transform

Geometry:
  ✅ Apply Modifiers
```

**Why this matters:** Without these settings, your model will import into Unity with rotation values like (-89.98, 0, 0) and scale values like (100, 100, 100) instead of clean defaults of (0, 0, 0) and (1, 1, 1). This makes it much harder to work with in Unity.

---

## Workflow: Creating a New Asset

### Step 1: Check Current Tasks

Look at the project board (Trello/Notion/etc.) for your assigned task.

**Example Task:** "Create wheelchair prop for patient rooms"

### Step 2: Create Branch in Anchorpoint

1. Open Anchorpoint
2. Make sure you're on the `main` branch and it's up to date
3. Click **"Create Branch"**
4. Name it: `art/wheelchair` (use format: `art/feature-name`)
5. Anchorpoint switches you to this branch automatically

**Branch Naming:**
- `art/wheelchair` - Single prop
- `art/patient-room-furniture` - Group of related assets
- `art/monster-asylum-animations` - Animation set
- `art/ui-inventory` - UI system

### Step 3: Create Your Folders

**In `art-source/`:**
```
art-source/props/wheelchair/
```

Create your working files here:
- `wheelchair-model.blend`
- `wheelchair-textures.spp`

**In `art-export/`:**
```
art-export/props/wheelchair/
```

This stays empty until you're ready to export.

### Step 4: Work on Your Art

Use your preferred tools. Save your work regularly in `art-source/`.

Anchorpoint will show your changes - you can commit your working files anytime to save progress.

### Step 5: Export Game-Ready Assets

When your asset is ready:

1. **Export model** to `art-export/props/wheelchair/SM_Wheelchair_01.fbx`
2. **Export textures** to:
   - `T_Wheelchair_BaseColor.png`
   - `T_Wheelchair_Normal.png`
   - `T_Wheelchair_Roughness.png`
3. **Export icon** (if needed): `UI_Icon_Wheelchair.png`

### Step 6: Commit in Anchorpoint

1. Anchorpoint shows changed files
2. Check both `art-source/` and `art-export/` files
3. Write a clear commit message:
   - ✅ "Added wheelchair prop with textures"
   - ✅ "Updated wheelchair model - reduced polycount"
   - ❌ "stuff" or "changes"
4. Click **Commit**

**Git Hook Protection:** If you named files incorrectly, the commit will be rejected with an error message explaining what's wrong. Fix the names and try again.

### Step 7: Push & Create Pull Request

1. Click **Push** in Anchorpoint
2. Go to GitHub/GitLab
3. Create a **Pull Request** from `art/wheelchair` → `main`
4. Add description: What you made, any notes
5. Tag the programmer for review

### Step 8: Feedback & Iteration

The programmer will review and may ask for changes:
- "Can you reduce the polycount?"
- "Normal map looks inverted"
- "Can we get a rusty variant?"

Make changes, commit, push - the PR updates automatically.

### Step 9: Merge & Cleanup

Once approved:
1. Programmer merges your branch
2. Your asset is now in `main`
3. Delete your branch (Anchorpoint can do this)
4. Switch back to `main` branch
5. Pull latest changes

---

## Common Workflows

### Multiple Assets in One Branch

**Task:** Create all furniture for patient room

**Branch:** `art/patient-room-furniture`

**Structure:**
```
art-source/environments/patient-room/
├── bed-variants.blend
├── cabinet.blend
├── chair.blend
└── furniture-textures.spp

art-export/environments/patient-room/
├── SM_Bed_01.fbx
├── SM_Bed_02.fbx
├── SM_Cabinet_01.fbx
├── SM_Chair_01.fbx
├── T_Furniture_BaseColor.png
└── T_Furniture_Normal.png
```

Commit all together when the set is complete.

### Updating an Existing Asset

**Need to fix the wheelchair?**

1. Create new branch: `art/wheelchair-fix`
2. Modify files in `art-source/props/wheelchair/`
3. Re-export to `art-export/props/wheelchair/`
4. Commit with message: "Fixed wheelchair UV stretching"
5. Push & create PR

### Creating Variants

**Need 3 dead tree variations?**

```
art-source/nature/
├── dead-tree-01/
│   └── tree-model.blend
├── dead-tree-02/
│   └── tree-model.blend
└── dead-tree-03/
    └── tree-model.blend

art-export/nature/
├── dead-tree-01/
│   ├── SM_DeadTree_01.fbx
│   └── T_DeadTree_01_BaseColor.png
├── dead-tree-02/
│   ├── SM_DeadTree_02.fbx
│   └── T_DeadTree_02_BaseColor.png
└── dead-tree-03/
    ├── SM_DeadTree_03.fbx
    └── T_DeadTree_03_BaseColor.png
```

---

## Quality Checklist

Before pushing your assets, verify:

### 3D Models
- [ ] Correct naming convention (SM_, SK_, AM_)
- [ ] Clean topology (no n-gons or overlapping faces)
- [ ] Proper scale (1 unit = 1 meter)
- [ ] Applied transforms (location, rotation, scale)
- [ ] Correct pivot point placement
- [ ] Within polycount budget
- [ ] Clean UVs (no overlapping unless intended)
- [ ] No unnecessary objects in scene (cameras, lights)

### Textures
- [ ] Correct naming (T_ObjectName_Type)
- [ ] Proper resolution (power of 2 if possible)
- [ ] Correct format (PNG or TGA)
- [ ] Normal maps are in DirectX format
- [ ] Metallic/Roughness are grayscale
- [ ] No lighting baked into BaseColor

### Animations
- [ ] Correct naming (AM_ObjectName_Action)
- [ ] Proper frame range
- [ ] Root motion baked (if applicable)
- [ ] No unnecessary keyframes
- [ ] Smooth transitions

### UI
- [ ] Correct naming (UI_ prefix)
- [ ] Transparent background where needed
- [ ] Correct resolution
- [ ] Readable at target size

---

## Troubleshooting

### "Git rejected my commit"

The pre-commit hook found naming errors. Read the error message carefully:

```
❌ Invalid model name: wheelchair.fbx
   Must be: SM_ObjectName_01.fbx
```

Fix the filename and commit again.

### "I accidentally committed to main"

Don't panic! Tell the programmer - they can help fix it.

### "My files are too large to push"

We use Git LFS for large files. It should be automatic, but if you see errors, let the programmer know.

### "I need to work on two features at once"

Don't! Finish one branch, merge it, then start the next. If you must, create separate branches for each feature.

### "Anchorpoint is confusing"

Ask for help! We can do a screen-share walkthrough.

---

## Best Practices

### DO ✅
- Create a new branch for each feature/task
- Commit your progress regularly (even work-in-progress)
- Write clear commit messages
- Keep related assets together in one folder
- Follow naming conventions exactly
- Export with correct settings
- Test your exports (open them to verify)
- Ask questions if unsure

### DON'T ❌
- Work directly on `main` branch
- Commit broken/incomplete exports
- Use generic file names
- Mix unrelated assets in one commit
- Push without checking file names
- Ignore polycount/texture budgets
- Forget to push your commits

---

## Scale Reference

**Important:** Unity uses 1 unit = 1 meter

Model your assets to these real-world scales:

| Object | Approximate Size |
|--------|------------------|
| Human Character | 1.7-1.8m tall |
| Door | 2.0m tall × 0.9m wide |
| Standard Bed | 2.0m × 1.5m |
| Wheelchair | 0.9m wide × 1.2m tall |
| Flashlight | 0.2m long |
| Keys | 0.05-0.08m long |

When in doubt, model to real-world scale and it'll be correct in Unity.

---

## Getting Help

**Questions about:**
- **Anchorpoint/Git:** Ask the programmer
- **Export settings:** Check this guide or ask programmer
- **Design decisions:** Check concept art or ask designer
- **Technical limits:** Ask programmer about polycount/texture limits
- **Naming conventions:** This document has examples

**Feedback Loop:**
- Pull Requests are your friend
- Don't be afraid of feedback - it makes the game better
- Iteration is normal and expected

---

## Quick Reference Card

```
BRANCHES:
Create: art/feature-name
Example: art/wheelchair, art/patient-room-props

FOLDERS:
Work: art-source/category/object-name/
Export: art-export/category/object-name/

NAMING:
Models: SM_ObjectName_01.fbx
Skeletal: SK_ObjectName_01.fbx  
Animations: AM_ObjectName_Action.fbx
Textures: T_ObjectName_Type.png
UI: UI_ElementName_State.png

WORKFLOW:
1. Create branch in Anchorpoint
2. Work in art-source/
3. Export to art-export/
4. Commit both
5. Push & create PR
6. Wait for review
7. Merge & delete branch
```

---

**Questions?** Contact [Programmer Name] on [Discord/Slack/Email]

**Last Updated:** [Date]
