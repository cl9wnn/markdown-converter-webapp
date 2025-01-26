function customAlert(message) {
    const overlay = document.createElement('div');
    const alertBox = document.createElement('div');
    const alertText = document.createElement('p');
    const closeButton = document.createElement('button');

    alertText.textContent = message;
    closeButton.textContent = 'OK';

    overlay.classList.add('custom-alert-overlay');
    alertBox.classList.add('custom-alert-box');
    closeButton.classList.add('custom-alert-button');

    alertBox.appendChild(alertText);
    alertBox.appendChild(closeButton);
    overlay.appendChild(alertBox);

    document.body.appendChild(overlay);

    closeButton.addEventListener('click', () => {
        alertBox.classList.add('fade-out'); 
        setTimeout(() => {
            document.body.removeChild(overlay);
        }, 300); 
    });
}