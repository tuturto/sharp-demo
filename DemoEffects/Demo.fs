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
 
    override this.Initialize() =
        do spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        do base.Initialize()
        ()
 
    override this.LoadContent() =
        ()
 
    override this.Update (gameTime) =
        ()
 
    override this.Draw (gameTime) =
        do this.GraphicsDevice.Clear Color.CornflowerBlue
        ()
