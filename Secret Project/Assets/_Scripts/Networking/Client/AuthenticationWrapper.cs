using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace _Scripts.Networking.Client
{
    public static class AuthenticationWrapper
    {
        public static AuthState AuthState { get; private set; } = AuthState.NotAuthenticated;

        public static async Task<AuthState> DoAuthAsync(int maxTries = 5)
        {
            if (AuthState == AuthState.Authenticated)
            {
                return AuthState;
            }

            if (AuthState == AuthState.Authenticating)
            {
                Debug.LogError("Already Authenticating...");
                return await AuthenticatingAsync();
            }

            await SignInAnonymouslyAsync(maxTries);

            return AuthState;
        }

        private static async Task SignInAnonymouslyAsync(int maxTries)
        {
            AuthState = AuthState.Authenticating;

            int tries = 0;
            while (AuthState == AuthState.Authenticating && tries < maxTries)
            {
                try
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();

                    if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                    {
                        AuthState = AuthState.Authenticated;
                        break;
                    }
                }
                catch (AuthenticationException authException)
                {
                    Debug.LogError(authException);
                    AuthState = AuthState.Error;
                }
                catch (RequestFailedException requestException)
                {
                    Debug.LogError(requestException);
                    AuthState = AuthState.Error;
                }

                tries++;
                await Task.Delay(1000);
            }

            if (AuthState != AuthState.Authenticated)
            {
                Debug.LogWarning($"Player was not signed in after {tries} tries ");
                AuthState = AuthState.TimeOut;
            }
        }

        private static async Task<AuthState> AuthenticatingAsync()
        {
            while (AuthState == AuthState.Authenticating || AuthState == AuthState.NotAuthenticated)
            {
                await Task.Delay(200);
            }

            return AuthState;
        }
    }

    public enum AuthState
    {
        NotAuthenticated,
        Authenticating,
        Authenticated,
        Error,
        TimeOut
    }
}