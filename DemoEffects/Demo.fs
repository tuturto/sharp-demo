module Demo

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type Colour =
    { r : int;
      g : int;
      b : int }

type Location = 
    { x : int;
      y : int }

type PlasmaPoint =
    { location: Location;
      v : float; }

type ColourPoint =
    { location : Location;
      colour : Colour }

type Game () as this =
    inherit Microsoft.Xna.Framework.Game()
 
    do this.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let mutable plasmaState = None
 
    let pixel = lazy this.Content.Load<Texture2D> "pixel"
    let pixelSize = 5
    let width = 800 / pixelSize
    let height = 600 / pixelSize
    let width' = (float)width
    let height' = (float)height

    let pointList =
        [ for x in 0..width do
            yield [for y in 0..height do
                    yield { x = x * pixelSize; y = y * pixelSize; } ] ]
        |> Seq.concat
        |> List.ofSeq

    let plasmaOne tick point =
        let x' = (float)point.x / width'
        let y' = (float)point.y / height'
        let cx = x' + 0.5 * sin ( tick / 5.0)
        let cy = y' + 0.5 * cos ( tick / 3.0)
        { location = point;
          v = sin ( x' * 2.0 + tick ) +
              sin (2.0 * (x' * sin (tick / 2.0) + y' * cos (tick / 3.0)) + tick) +
              sin ( sqrt ( 10.0 * ( cx*cx + cy*cy ) ) + 0.5 + tick)
              ;}

    let colour ( v : PlasmaPoint ) =
        { location = v.location; 
          colour = { r = (int)((sin ( v.v * (float)MathHelper.Pi ) + 1.0) * 128.0);
                     g = (int)((cos ( v.v * (float)MathHelper.Pi ) + 1.0) * 128.0);
                     b = 0; } }

    let plasma tick =
        let vList = List.map (plasmaOne tick) pointList
        List.map colour vList

    let drawPoint point =
        let pixel' = pixel.Force()
        spriteBatch.Draw(pixel', Rectangle(point.location.x, point.location.y, pixelSize, pixelSize), Color(point.colour.r, point.colour.g, point.colour.b))

    override this.Initialize() =
        do base.Initialize()
        do spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        ()
 
    override this.LoadContent() =
        pixel.Force() |> ignore
        ()
 
    override this.Update (gameTime) =
        let diff = (float)gameTime.TotalGameTime.TotalSeconds
        do plasmaState <- Some (plasma diff)
        ()
 
    override this.Draw (gameTime) =        
        match plasmaState with
            | None -> ()
            | Some state -> do
                                spriteBatch.Begin()
                                List.iter drawPoint state
                                spriteBatch.End()
        ()
