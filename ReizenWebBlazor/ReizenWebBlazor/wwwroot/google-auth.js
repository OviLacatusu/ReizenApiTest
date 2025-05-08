var codeClient;

window.googleAuth = {
    init: function (clientId, scopes, redirectUri, dotNetRef) {
        codeClient = google.accounts.oauth2.initCodeClient({
            client_id: clientId,
            scope: scopes.join(' '),
            ux_mode: 'redirect',
            callback: (response) => {
                dotNetRef.invokeMethodAsync('HandleCode', response.code);
            }
        });
    },

    requestCode: function () {
        if (codeClient) {
            codeClient.requestCode();
        } else {
            console.error('Google Auth client not initialized.');
        }
    }
};