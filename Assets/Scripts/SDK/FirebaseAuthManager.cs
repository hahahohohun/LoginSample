using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class FirebaseAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Create()
    {

    }

    public void Login()
    {

    }

    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("·Î±×¾Æ¿ô");
    }
}