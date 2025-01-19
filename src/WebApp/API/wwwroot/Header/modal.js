export function createModal(saveProjectCallback) {
    const modal = document.createElement('div');
    modal.className = 'modal';

    const modalOverlay = document.createElement('div');
    modalOverlay.className = 'modal-overlay';

    const modalContent = document.createElement('div');
    modalContent.className = 'modal-content';

    const modalTitle = document.createElement('h2');
    modalTitle.textContent = 'Save Project';

    const titleInput = document.createElement('input');
    titleInput.type = 'text';
    titleInput.id = 'projectTitle';
    titleInput.placeholder = 'Enter project title';
    titleInput.className = 'modal-input';

    const saveButton = document.createElement('button');
    saveButton.textContent = 'Save';
    saveButton.className = 'modal-save-button';
    saveButton.addEventListener('click', async () => {
        const title = titleInput.value.trim();
        if (title) {
            await saveProjectCallback(title);
            closeModal(modal, modalOverlay);
        } else {
            alert('Please enter a title for the project.');
        }
    });

    const closeIcon = document.createElement('button');
    closeIcon.className = 'modal-close-icon';
    closeIcon.innerHTML = '&times;';
    closeIcon.addEventListener('click', () => {
        closeModal(modal, modalOverlay);
    });

    modalContent.appendChild(modalTitle);
    modalContent.appendChild(titleInput);
    modalContent.appendChild(saveButton);
    modal.appendChild(closeIcon);
    modal.appendChild(modalContent);

    document.body.appendChild(modalOverlay);
    document.body.appendChild(modal);
}

function closeModal(modal, overlay) {
    modal.remove();
    overlay.remove();
}