using Godot;
using System;
using System.Linq;

[GlobalClass]
public partial class BuildingComponent : Node3D
{
	// сцены башен для установки
	[Export]
	private Godot.Collections.Array<PackedScene> TowerScenes = new Godot.Collections.Array<PackedScene>();
	// стоимость башни
	[Export]
	private float TowerCost = 100f;
	// предпросмотр башен перед их установкой
	[Export]
	private Godot.Collections.Array<CharacterBody3D> PreviewsTowers = new Godot.Collections.Array<CharacterBody3D>();
	// выбранный тип башни
	private int selectedTowerType = 0;

	// определение, находится ли игрок в режиме строительства
	private bool IsBuildingMode()
	{
		return Visible;
	}

	// в начале игры 
    public override void _Ready()
    {
		UpdatePreview(0);
    }

	// выбор башни в зависимости от клавиши клавиатуры
    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("build")) Visible = !Visible;

		if (IsBuildingMode())
		{
			if (Input.IsActionJustPressed("select_normal_tower")) SelectTowerType(0);
			if (Input.IsActionJustPressed("select_explosion_tower")) SelectTowerType(1);
			if (Input.IsActionJustPressed("select_freeze_tower")) SelectTowerType(2);

			if (Input.IsActionJustPressed("accept")) Place();

		}
    }

	// выбор типа башни
	private void SelectTowerType(int idx)
	{
		if (TowerScenes != null && idx >= 0 && idx < TowerScenes.Count)
		{
			selectedTowerType = idx;
			UpdatePreview(idx);
		}
	}

	private void UpdatePreview(int idx)
	{
		if (PreviewsTowers == null) return;
		else
		{
			for (int i = 0; i < PreviewsTowers.Count; i++)
			{
				if (PreviewsTowers[i] != null)
				{
					if (i == idx) PreviewsTowers[i].Visible = true;
					else PreviewsTowers[i].Visible = false;
				}
			}
		}
	}

	private void Place()
	{
		if (GameManager.Instance.SpendMetal(TowerCost))
		{
			CharacterBody3D newTower = TowerScenes[selectedTowerType].Instantiate<CharacterBody3D>();
			GetTree().CurrentScene.AddChild(newTower);
			
			// Так как этот компонент привязан к игроку, 
			// его GlobalPosition — это и есть нужная точка перед игроком
			newTower.GlobalPosition = this.GlobalPosition;
			
			Visible = false; // Выходим из режима постройки
		}
	}
}
