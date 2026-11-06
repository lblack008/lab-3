using Godot;
using System;

public partial class GameManager : Node3D
{
	public static GameManager Instance {get; private set;}

	[Export] 
	public float Metal {get; private set;} = 200f;
	[Export] 
	private Label metalLabel;
	[Export] 
	private Label noMetalLabel;
	private RandomNumberGenerator _random = new RandomNumberGenerator();
	[Export]
	private Godot.Collections.Array<PackedScene> _objects = new Godot.Collections.Array<PackedScene>();
	private Godot.Collections.Array<Vector3> _positions = new Godot.Collections.Array<Vector3>();

	private Timer _timer;

	public override void _Ready()
	{
		_positions.Add(new Vector3(-6.901f, -0.323f, 0.068f));
		_positions.Add(new Vector3(-5.747f, -0.323f, 2.841f));
		_positions.Add(new Vector3(-0.796f, -0.323f, -4.39f));
		_random.Randomize();
		_timer = GetNode<Timer>("Timer");
		Instance = this;
		UpdateUI();
	}
	
	//спавн врагов через определённое время
	private void OnSpawnTimerTimeout()
	{
		SpawnEnemy();
	}

	//спавн врагов случайным образом
	private void SpawnEnemy()
	{
		int randomIndex = _random.RandiRange(0, _objects.Count - 1);
		int randomPositionIdx = _random.RandiRange(0, _positions.Count - 1);

		//сообщение в консоль, что был заспавнен враг
		GD.Print($"Spawn enemy: {_objects[randomIndex].ResourcePath}");
		CharacterBody3D obj = (CharacterBody3D)_objects[randomIndex].Instantiate();
		
		obj.Position = _positions[randomPositionIdx];
		AddChild(obj);
	}

	public bool SpendMetal(float count)
	{
		if (Metal >= count)
		{
			Metal -= count;
			UpdateUI();
			return true;
		}
		
		Error();
		return false;
	}

	public void AddMetal(float count)
	{
		Metal += count;
		UpdateUI();
	}

	private void UpdateUI()
	{
		if (metalLabel != null) metalLabel.Text = $"Металл: {Metal}";
	}

	private void Error()
	{
		if (noMetalLabel != null)
		{
			noMetalLabel.Visible = true;
			metalLabel.Visible = false;
			// Спрятать ошибку через 2 секунды
			GetTree().CreateTimer(2.0f).Timeout += () => noMetalLabel.Visible = false;
		}
	}

	public void GameOver()
	{
		GD.Print("Game Over!");
		GetTree().CallDeferred(SceneTree.MethodName.ReloadCurrentScene); 
	}
}
