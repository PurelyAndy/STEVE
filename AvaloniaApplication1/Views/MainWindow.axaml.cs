using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace AvaloniaApplication1.Views;

public partial class MainWindow : Window
{
    private readonly List<(string Text, double Size)> _bingoItems =
    [
        ("It's quiet... <i>too<i> quiet...", 30),
        ("I have a bad feeling about this.", 25),
        ("Let's get out of here!", 30),
        ("He's right behind me, isn't he?", 30),
        ("That sounded better in my head.", 25),
        ("That's what I'm talking about!", 25),
        ("That's gonna leave a mark.", 27),
        ("<i>Cut to montage<i>", 38),
        ("We can do this the easy way, or the hard way.", 25),
        ("Aw man", 50),
        ("Let's kick it up a <i>notch!<i>", 30),
        ("Is that all you got?", 30),
        ("Try me!", 50),
        ("I'm just getting started.", 30),
        ("Are you thinking what I'm thinking?", 25),
        ("We've got company!", 35),
        ("We... are Minecraft", 35),
        ("What... just happened?", 30),
        ("Right in the ender pearls!", 30),
        ("It's gonna blow!", 35),
        ("<i>Nobody move!<i>", 40),
        ("<i>Thunderstruck by AC/DC<i>", 30),
        ("And you... well, you just keep being you.", 25),
        ("<i>Jack black sings something<i>", 25),
        ("You better come take a look at this!", 25),
        ("<i>(With weapons in a circle)<i> On three!", 25),
        ("Kiss my axe!", 40),
        ("Block head!", 50),
        ("<i>Youtuber reference (jack black doesn't count)<i>", 20),
        ("<i>Crafting an item that doesn't exist in-game<i>", 23),
        ("<i>Crafting recipe for item doesn't match in-game recipe<i>", 20),
        ("That. Was. AWESOME!!!", 30),
        ("<i>Alex shows up at the end as sequel bait<i>", 25),
        ("<i>Character unable to communicate with villagers<i>", 20),
        ("<i>Creepers are a recurring joke<i>", 25),
        ("So... that just happened...", 30),
        ("<i>Idiot looks an enderman in the eyes<i>", 25),
        ("<i>Character questions Minecraft mechanics<i>", 25),
        ("<i>Minecart chase<i>", 35),
        ("<i>Character dies and respawns<i>", 30),
        ("<i>Outdated reference<i>", 30),
        ("Bad dog!", 50),
        ("<i>Two characters scare and scream at each other<i>", 20),
        ("...Guys? You might wanna see this...", 25),
        ("Not bad, kid.", 35),
        ("____ is my middle name!", 30),
        ("I got it, I got it... I don't got it!", 25),
        ("<i>Die here, die IRL situation<i>", 35),
        ("That's gotta hurt...", 35),
        ("I'm too old for this.", 35)
    ];

    private int _counter;

    public MainWindow()
    {
        InitializeComponent();

        RollBingo();
    }

    private void RollBingo()
    {
        while (MainCanvas.Children.Count > 1)
            MainCanvas.Children.RemoveAt(0);
        while (MainGrid.Children.Count > 1)
            MainGrid.Children.RemoveAt(1);
        
        for (int i = 0; i < 20; i++)
        {
            double minDegreesFrom90 = 30;
            double rand1;
            do
            {
                rand1 = Random.Shared.NextDouble() * 360;
            } while (rand1 > 90 - minDegreesFrom90 && rand1 < 90 + minDegreesFrom90 ||
                     rand1 > 270 - minDegreesFrom90 && rand1 < 270 + minDegreesFrom90);

            double rand2;
            do
            {
                rand2 = Random.Shared.NextDouble() * 360;
            } while (rand2 > 90 - minDegreesFrom90 && rand2 < 90 + minDegreesFrom90 ||
                     rand2 > 270 - minDegreesFrom90 && rand2 < 270 + minDegreesFrom90);

            double rand3 = Random.Shared.NextDouble() * 360;
            Image img = new()
            {
                Source = new Bitmap(AssetLoader.Open(new("avares://AvaloniaApplication1/Assets/blackjack.png"))),
                RenderTransform = new Rotate3DTransform(
                    rand1,
                    rand2,
                    rand3,
                    0,
                    0,
                    0,
                    220)
            };
            
            switch (i)
            {
                case < 7: // If the image is in the top row
                    img.SetValue(Canvas.TopProperty, -40);
                    break;
                case > 9 and < 17: // If the image is in the bottom row
                    img.SetValue(Canvas.TopProperty, 950);
                    break;
                default:
                    img.SetValue(Canvas.TopProperty, -200 + (i % 5) * 218);
                    break;
            }

            switch (i)
            {
                case > 16 and < 20: // If the image is in the left column
                    img.SetValue(Canvas.LeftProperty, -50);
                    break;
                case > 5 and < 11: // If the image is in the right column
                    img.SetValue(Canvas.LeftProperty, 850);
                    break;
                default:
                    img.SetValue(Canvas.LeftProperty, -50 + (i % 6) * 148);
                    break;
            }

            MainCanvas.Children.Insert(0, img);
        }

        for (int i = 0; i < _bingoItems.Count; i++)
        {
            int swapped = Random.Shared.Next(i, _bingoItems.Count);
            (_bingoItems[i], _bingoItems[swapped]) = (_bingoItems[swapped], _bingoItems[i]);
        }

        for (int i = 1; i <= 5; i++)
        for (int j = 1; j <= 5; j++)
        {
            double width = 6;
            double left;
            double top;
            double bottom;
            double right;

            switch (i)
            {
                case 1: // Box is on the left, so the left border is thicker
                    left = width;
                    right = width / 2;
                    break;
                case 5: // Box is on the right, so the right border is thicker
                    left = width / 2;
                    right = width;
                    break;
                default:
                    left = width / 2;
                    right = width / 2;
                    break;
            }

            switch (j)
            {
                case 1: // Box is on the top, so the top border is thicker
                    top = width;
                    bottom = width / 2;
                    break;
                case 5: // Box is on the bottom, so the bottom border is thicker
                    top = width / 2;
                    bottom = width;
                    break;
                default:
                    top = width / 2;
                    bottom = width / 2;
                    break;
            }

            Thickness thickness = new(left, top, right, bottom);
            
            string[] split = _bingoItems[(i - 1) * 5 + j - 1].Text.Split("<i>");
            InlineCollection parts = new();
            for (int k = 0; k < split.Length; k++)
            {
                parts.Add(new Run(split[k])
                {
                    FontStyle = k % 2 == 1 ? FontStyle.Italic : FontStyle.Normal
                });
            }

            Border border = new()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = thickness,
                Child = new TextBlock
                {
                    FontSize = _bingoItems[(i - 1) * 5 + j - 1].Size,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    Inlines = parts
                }
            };
            Grid.SetColumn(border, i);
            Grid.SetRow(border, j);
            MainGrid.Children.Add(border);
        }
    }

    private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (e.InitialPressMouseButton == MouseButton.Left)
            RollBingo();
        else
            SaveScreenshot();
    }

    private void SaveScreenshot()
    {
        PixelSize size = new((int)MainCanvas.Bounds.Width, (int)MainCanvas.Bounds.Height);
        RenderTargetBitmap renderTarget = new(size);
        renderTarget.Render(MainCanvas);
        using FileStream file = new($"bingo{_counter++}.png", FileMode.Create);
        renderTarget.Save(file, 100);
    }
}