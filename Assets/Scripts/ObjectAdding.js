#pragma strict


var target : GameObject;


function Start()
{
    InvokeRepeating("SpawnObject", 10, 30);
}

function SpawnObject()
{
    var x : float = Random.Range(-0.5f, 0.7f);
    var z : float = Random.Range(-0.5f, 0.5f);
    Instantiate(target, new Vector3(x, 2.5, z), Quaternion.identity);
}