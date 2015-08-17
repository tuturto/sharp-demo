module Demo

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type Game () as this =
    inherit Microsoft.Xna.Framework.Game()
 
    do this.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
 
    let pixel = lazy this.Content.Load<Texture2D> "pixel"

    override this.Initialize() =
        do base.Initialize()
        do spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        ()
 
    override this.LoadContent() =
        pixel.Force() |> ignore
        ()
 
    override this.Update (gameTime) =
        ()
 
    override this.Draw (gameTime) =
        do this.GraphicsDevice.Clear Color.CornflowerBlue
        spriteBatch.Begin()
        spriteBatch.Draw(pixel.Force(), Rectangle(10, 10, 10, 10), Color(25, 250, 45))
        spriteBatch.End()
        ()
