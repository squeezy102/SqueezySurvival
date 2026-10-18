# Art Direction

**Status:** Draft - first pass, to be judged in play ([#50](https://github.com/squeezy102/SqueezySurvival/issues/50))

How the game looks: 16-bit pixel art in the spirit of Chrono Trigger, Final Fantasy VI and Stardew Valley, with a warm, hand-made interface in the spirit of Harvest Moon, Stardew Valley and Terraria. It is not a full SNES pastiche: no blue boxes with white borders.

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
| Panel | A wood frame: a 1-pixel dark outline, then a 2-pixel wood rim, light along the top and left and dark along the bottom and right. Inside, a deep warm brown, slightly see-through. Cream text. |
| Window (pause, prompts, Options) | The panel frame, fully opaque. |
| Button | A raised tan block: 1-pixel dark outline and a 1-pixel bevel, light top and left, dark bottom and right. Dark brown text. Brighter while hovered, darker while pressed, faded while unavailable. |
| Slot | A sunken square: the bevel reversed, dark top and left, light bottom and right, so slots read as holes items sit in. |
| Item cell | No frame of its own: the icon sits in its slot. |
| Tooltip | Parchment: a cream block with a dark outline and dark brown text, like Stardew Valley's tooltips. |

### Palette

| Name | RGB | Used for |
|---|---|---|
| Outline | 42, 24, 16 | Frame outlines |
| Wood light | 196, 136, 76 | Rim highlight |
| Wood | 150, 96, 52 | Rim |
| Wood dark | 98, 58, 32 | Rim shadow |
| Panel fill | 52, 34, 26, at 92% opacity | Panel interiors |
| Tan light | 246, 214, 160 | Button highlight |
| Tan | 222, 182, 122 | Button face |
| Tan dark | 170, 126, 74 | Button shadow |
| Slot | 34, 22, 16 | Slot interiors |
| Parchment | 250, 236, 200 | Tooltip face |
| Cream | 250, 236, 200 | Text on dark |
| Ink | 58, 36, 22 | Text on light |

## Not yet decided

- A pixel font. The look would gain most from one; choosing one needs a font with a license that allows shipping, credited in [Asset Credits](../Engineering/AssetCredits.md).
- Background and scene art.

## Implementation notes

- Icons and frames are drawn as pixel images in Core (`PixelImage`, `PixelIconCanvas`, `PixelIconStylizer`, `PixelFrameImageFactory`) and unit tested; the presentation layer turns each into a texture with no smoothing and caches it (`PixelArtTextureCache`). No image files are needed.
- Frames are nine-sliced, so they stretch to any size without distorting their borders ([Unity Manual: 9-slice](https://docs.unity3d.com/Manual/UIB-styling-ui-backgrounds.html)).
