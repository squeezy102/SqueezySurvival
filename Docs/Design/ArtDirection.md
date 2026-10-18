# Art Direction

**Status:** Draft - first pass, to be judged in play ([#50](https://github.com/squeezy102/SqueezySurvival/issues/50))

How the game looks: 16-bit pixel art in the spirit of Chrono Trigger, Final Fantasy VI and Stardew Valley, with an interface of see-through navy glass and gold trim, in the spirit of Chrono Trigger's windows. It is not a full SNES pastiche: no opaque blue boxes with white borders, and the scene always shows through.

## Principles

- **Pixels are visible and crisp.** Art is drawn on a small pixel grid and scaled up by whole numbers with no smoothing, so every art pixel is a sharp square on screen ([Unity Scripting API: FilterMode.Point](https://docs.unity3d.com/ScriptReference/FilterMode.Point.html)).
- **Shapes stay simple.** The placeholder icons' shapes and colors are kept; only how they are drawn changes.
- **Readable first.** Text and icons keep strong contrast against what is behind them. Color is never the only signal ([HUD](UI/HUD.md)).

## Icons

Every item and zone feature icon is drawn on a **16 x 16 pixel** grid, like the item icons of the games above.

| Rule | Value |
|---|---|
| Grid | 16 x 16 art pixels. The shape fills the inner 14 x 14, leaving room for the outline. |
| Drawing | A pixel is filled when its center is inside a shape. Lines are at least 1 pixel wide. |
| Light | From the top left. A filled pixel whose upper or left neighbor is a different color, or empty, is lightened by 25%. One whose lower or right neighbor is different, or empty, is darkened by 25%. |
| Outline | Every empty pixel beside a filled one (up, down, left or right) becomes an outline in the darkest neighboring color, darkened by 65%. Outlines are colored, not black, as in Stardew Valley and Chrono Trigger. |
| Scaling | Whole multiples of 16 where the cell allows, with no smoothing. |

The shapes and colors of each placeholder icon live in one catalog (`PlaceholderIconCatalog`); anything without an icon shows a magenta square, so it is visible and obviously unfinished.

## Interface

Panels, buttons and slots are drawn as pixel-art frames, scaled up by **3** (each art pixel is 3 x 3 screen pixels at the 1920 x 1080 reference resolution).

| Element | Look |
|---|---|
| Panel | Navy glass: a 1-pixel dark outline, a 1-pixel gold rim (light along the top and left, dark along the bottom and right), then a 1-pixel navy band. Inside, deep navy at 65% opacity, so the scene shows through. Near-white text, gold headings. |
| Window (pause, prompts, Options, main menu) | The panel frame, nearly opaque (94%), so it reads over anything. |
| Button | A raised blue block: 1-pixel dark outline and a 1-pixel bevel, light top and left, dark bottom and right. White text. Lighter while hovered, sunk while pressed, faded while unavailable. |
| Slot | A sunken square: the bevel reversed, dark top and left, light bottom and right, slightly see-through, so slots read as holes items sit in. |
| Item cell | No frame of its own: the icon sits in its slot. |
| Tooltip | A pale card with a dark outline and navy text. |

### Palette

Chosen by squeezy102 from three options (navy glass, sea glass, slate) to replace the first pass's wood and parchment, which hid the scene ([#50](https://github.com/squeezy102/SqueezySurvival/issues/50)).

| Name | RGB | Used for |
|---|---|---|
| Outline | 14, 18, 40 | Frame outlines |
| Gold light | 222, 190, 110 | Rim highlight, headings |
| Gold dark | 150, 112, 52 | Rim shadow |
| Navy rim | 60, 80, 140 | Band inside the rim |
| Panel fill | 24, 36, 84, at 65% opacity | Panel interiors |
| Window fill | 24, 36, 84, at 94% opacity | Window interiors |
| Button light | 120, 150, 220 | Button highlight |
| Button | 70, 96, 170 | Button face |
| Button dark | 40, 56, 110 | Button shadow, pressed face |
| Slot | 10, 16, 40, at 75% opacity | Slot interiors |
| Tooltip | 236, 240, 250 | Tooltip face |
| Text | 240, 242, 250 | Text on navy |
| Ink | 30, 34, 60 | Text on the tooltip |

## Backgrounds

Scenes are pixel art too, drawn at a quarter of the reference resolution (**480 x 270**) and scaled by 4 with no smoothing, so their pixels match the interface's.

- Skies and water are flat bands of color, ordered-dithered where one band meets the next, as in 16-bit games.
- Objects in the scene (the palm) are drawn on their own layer and shaded and outlined by the icon rules.
- The placeholder beach is painted by the game (`PlaceholderBeachScenePainter`) and needs no image file. It replaced a smooth painted image, which looked out of place beside the pixel art; reducing that image automatically muddied its colors.

## Font

All interface text is set in **Pixelify Sans** ([#53](https://github.com/squeezy102/SqueezySurvival/issues/53)), a pixel font in the spirit of 16-bit games that stays readable at body sizes. It is licensed under the SIL Open Font License, which allows shipping it with the game ([Asset Credits](../Engineering/AssetCredits.md)). It is set once on each screen's root element and inherited by everything on it.

## Not yet decided

- Real scene art, and whether scenes change with the time of day.

## Implementation notes

- Icons and frames are drawn as pixel images in Core (`PixelImage`, `PixelIconCanvas`, `PixelIconStylizer`, `PixelFrameImageFactory`) and unit tested; the presentation layer turns each into a texture with no smoothing and caches it (`PixelArtTextureCache`). No image files are needed.
- The font is set with USS `-unity-font-definition` pointing at the font file, as Unity's App UI package does ([App UI: custom typography](https://docs.unity3d.com/Packages/com.unity.dt.app-ui@2.0/manual/custom-typography.html)); it takes precedence over `-unity-font` ([Unity Manual: USS text properties](https://docs.unity3d.com/Manual/UIE-USS-SupportedProperties.html)).
- Frames are nine-sliced, so they stretch to any size without distorting their borders ([Unity Manual: 9-slice](https://docs.unity3d.com/Manual/UIB-styling-ui-backgrounds.html)).
