using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContentScript : MonoBehaviour
{
    private class Leader
    {
        public int Wins;
        public string Name;

        public Leader(int wins, string name)
        {
            Wins = wins;
            Name = name;
        }
        
    }
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject leaderPref;
    private List<Leader> _leaders;

    public void Awake()
    {
        _leaders = new List<Leader>();
    }

    public void AddLeader(string name, int wins)
    {
        var leader = new Leader(wins, name);
        _leaders.Add(leader);
    }

    public void SpawnLeaders()
    {
        _leaders.OrderBy(leader => leader.Wins);
        
        foreach(var leader in _leaders)
        {
            var spawnLeader = Instantiate(leaderPref, new Vector3 (0, 0, 0), Quaternion.identity);
            spawnLeader.transform.SetParent(content.transform);
            spawnLeader.GetComponent<LeaderScript>().SetInfo(leader.Name, leader.Wins);
        }
    }
}
