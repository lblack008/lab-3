using Godot;
using System;

// система управления игрой
public partial class GameManager : Node3D
{
	// переменная для обращение к классу в любой части проекта
	public static GameManager Instance {get; private set;}

	// количество металла (задаётся в начале игры, просто так изменить нельзя)
	[Export] 
	public float Metal {get; private set;} = 200f;
	// текст в игре для обозначения, сколько металла
	[Export] 
	private Label metalLabel;
	// текст для обозначения, что металла недостаточно для установки башни
	[Export] 
	private Label noMetalLabel;
	// рандомайзер
	private RandomNumberGenerator _random = new RandomNumberGenerator();
	//твраги для спавна
	[Export]
	private Godot.Collections.Array<PackedScene> _objects = new Godot.Collections.Array<PackedScene>();
	// позиции объектов для спавна
	private Godot.Collections.Array<Vector3> _positions = new Godot.Collections.Array<Vector3>();
	// таймер для спавна
	private Timer _timer;

	// в начале игры задаются основные параметры для игры
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

	// уменьшение количества металла при установке башни (или вывод ошибки)
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

	// добавление металла при убийстве врага
	public void AddMetal(float count)
	{
		Metal += count;
		UpdateUI();
	}

	// обновление количества металла 
	private void UpdateUI()
	{
		if (metalLabel != null) metalLabel.Text = $"Металл: {Metal}";
	}

	// вывод ошибки
	private void Error()
	{
		if (noMetalLabel != null)
		{
			noMetalLabel.Visible = true;
			metalLabel.Visible = false;
			// Спрятать ошибку через 2 секунды
			GetTree().CreateTimer(2.0f).Timeout += () => {
				noMetalLabel.Visible = false;
				metalLabel.Visible = true;
			};
		}
	}

	// окончание игры
	public void GameOver()
	{
		GD.Print("Game Over!");
		GetTree().CallDeferred(SceneTree.MethodName.ReloadCurrentScene); 
	}
}
