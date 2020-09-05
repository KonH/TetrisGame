using TetrisGame.State;

namespace TetrisGame.Service {
	public interface IGameLoop {
		IReadOnlyGameState State { get; }
		void Update(float dt);
		void UpdateInput(InputState input);
	}
}