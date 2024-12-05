using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update()
    {}
        public void Menu()
    {
        SceneManager.LoadSceneAsync(0);
        
    }

    public void PlayGame(string InGame)
    {
        SceneManager.LoadScene(InGame);
        
    }
    public void ExitMainMenu(string MainMenu)
    {
        SceneManager.LoadScene(MainMenu);
        
    }
    public void Option(string option)
    {
        SceneManager.LoadScene(option);
        
    }

    public void Credit(string credit)
    {
        SceneManager.LoadScene(credit);
        
    }
    public void Continue(string win){
        SceneManager.LoadScene(win);
    }
    public void LeaderBoard(string LeaderBoard)
    {
        ScoreManager.Instance.SaveCurrentScore();
        
        
        SceneManager.LoadScene(LeaderBoard);
        
    }
    
    // public void Pouse()
    // {
    //     SceneManager.LoadSceneAsync(4);
        
        
    // }

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();

    }
    public void MainnMenu(string mainMenu){
        SceneManager.LoadScene(mainMenu);
    }

}
