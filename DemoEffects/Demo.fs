module Demo

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Plasma
open Stars

type GameState =
    | NoState
    | Plasma of ColourPoint list
    | Stars of Star list

type SwitchState =
    | Switching
    | Ready

type Game () as this =
    inherit Microsoft.Xna.Framework.Game()
 
    do this.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let pixel = lazy this.Content.Load<Texture2D> "pixel"

    let mutable currentEffect = NoState
    let mutable switchStatus = Ready

    override this.Initialize() =
        do base.Initialize()
        do graphics.PreferredBackBufferWidth <- GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width
        do graphics.PreferredBackBufferHeight <- GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height
        do graphics.IsFullScreen <- true
        do graphics.ApplyChanges()
        do spriteBatch <- new SpriteBatch(this.GraphicsDevice)
 
    override this.LoadContent() =
        pixel.Force() |> ignore
 
    override this.Update (gameTime) = 
        match currentEffect with
            | NoState -> do currentEffect <- Plasma (plasmaUpdate gameTime)
            | Plasma _ -> do currentEffect <- Plasma (plasmaUpdate gameTime)
            | Stars state -> do currentEffect <- Stars (starsUpdate gameTime (Some state))

        let keyState = Keyboard.GetState()
        match keyState.IsKeyDown(Keys.Escape) with
            | true -> do base.Exit()
            | false -> do base.Update(gameTime)
        match switchStatus with
            | Ready ->
                match keyState.IsKeyDown(Keys.Space) with
                    | true -> match currentEffect with
                                  | NoState 
                                        -> do currentEffect <- Plasma (plasmaUpdate gameTime)
                                           do switchStatus <- Switching
                                  | Plasma _ 
                                        -> do currentEffect <- Stars (starsUpdate gameTime None)
                                           do switchStatus <- Switching
                                  | Stars _ 
                                        -> do currentEffect <- Plasma (plasmaUpdate gameTime)
                                           do switchStatus <- Switching
                    | false -> ()
            | Switching ->
                match keyState.IsKeyDown(Keys.Space) with
                    | true -> ()
                    | false -> do switchStatus <- Ready
 
    override this.Draw (gameTime) =        
        match currentEffect with
            | NoState -> ()
            | Plasma state -> do plasmaDraw spriteBatch (pixel.Force()) state
            | Stars state -> do starsDraw graphics.GraphicsDevice spriteBatch (pixel.Force()) state
