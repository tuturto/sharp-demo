module Demo

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Plasma

type Game () as this =
    inherit Microsoft.Xna.Framework.Game()
 
    do this.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let pixel = lazy this.Content.Load<Texture2D> "pixel"

    let mutable state = None

    override this.Initialize() =
        do base.Initialize()
        do graphics.PreferredBackBufferWidth <- GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width
        do graphics.PreferredBackBufferHeight <- GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height
        do graphics.IsFullScreen <- true
        do graphics.ApplyChanges()
        do spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        ()
 
    override this.LoadContent() =
        pixel.Force() |> ignore
        ()
 
    override this.Update (gameTime) = 
        match state with
            | None -> state <- Some (PlasmaEffect(spriteBatch, pixel.Force()))
            | Some state -> do state.Update gameTime

        let keyState = Keyboard.GetState()
        match keyState.IsKeyDown(Keys.Escape) with
            | true -> do base.Exit()
            | false -> do base.Update(gameTime)
        ()
 
    override this.Draw (gameTime) =        
        match state with
            | None -> ()
            | Some state -> do state.Draw(gameTime)
        ()
