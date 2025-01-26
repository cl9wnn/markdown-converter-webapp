import {createAccessModal} from "./accessModal.js";
import {giveAccess, documentId} from "./script.js";

export function createAccessSettingsModal(permissionArray, clearPermission) {
    console.log(permissionArray);

    const modal = document.createElement('div');
    modal.className = 'per-modal';

    const modalContent = document.createElement('div');
    modalContent.className = 'per-modal-content';

    const modalHeader = document.createElement('div');
    modalHeader.className = 'per-modal-header';

    const titleAndButtonContainer = document.createElement('div');
    titleAndButtonContainer.className = 'per-title-button-container';

    const title = document.createElement('h2');
    title.textContent = 'Users, who have access';

    const addButton = document.createElement('button');
    addButton.className = 'per-add-button';
    addButton.textContent = '+';
    addButton.onclick = async () => {
        modal.remove();
        await createAccessModal('Give access', async (inputData) => {
            const success = await giveAccess(inputData, documentId);
            if (success) {
                permissionArray.push({
                    email: inputData.email,
                    permissionName: inputData.permissionName,
                    documentId: inputData.documentId,
                });
                renderRoles();
            } else {
                console.error('Ошибка добавления роли');
            }
        });
    };

    titleAndButtonContainer.appendChild(title);
    titleAndButtonContainer.appendChild(addButton);

    const closeModalButton = document.createElement('button');
    closeModalButton.className = 'per-close-modal';
    closeModalButton.textContent = '×';

    closeModalButton.onclick = () => {
        modal.remove();
    };

    modalHeader.appendChild(titleAndButtonContainer);
    modalHeader.appendChild(closeModalButton);

    const modalBody = document.createElement('div');
    modalBody.className = 'per-modal-body';

    function renderRoles() {
        modalBody.innerHTML = '';

        if (permissionArray.length === 0) {
            const noPermissionsMessage = document.createElement('div');
            noPermissionsMessage.className = 'per-no-permissions';
            noPermissionsMessage.textContent = 'Permissions not found';
            modalBody.appendChild(noPermissionsMessage);
            return;
        }

        permissionArray.forEach((role, index) => {
            const listItem = document.createElement('div');
            listItem.className = 'per-list-item';

            const userInfoContainer = document.createElement('div');
            userInfoContainer.className = 'per-user-info';

            const emailElement = document.createElement('span');
            emailElement.className = 'per-email';
            emailElement.textContent = role.email;

            const roleElement = document.createElement('span');
            roleElement.className = 'per-role';
            roleElement.textContent = role.permissionName;

            userInfoContainer.appendChild(emailElement);
            userInfoContainer.appendChild(roleElement);

            const buttonContainer = document.createElement('div');
            buttonContainer.className = 'per-button-container';

            const changeButton = document.createElement('button');
            changeButton.className = 'per-change-button';
            changeButton.onclick = async () => {
                modal.remove();
                await createAccessModal('Give access', async (inputData) => {
                    await giveAccess(inputData, role.documentId);
                }, role.email);
            };

            const deleteButton = document.createElement('button');
            deleteButton.className = 'per-delete-button';
            deleteButton.textContent = '✖';
            deleteButton.onclick = async () => {
                const success = await clearPermission(role.documentId, role.email);
                if (success) {
                    permissionArray = permissionArray.filter((_, i) => i !== index);
                    renderRoles();
                } else {
                    console.error('Ошибка удаления роли');
                }
            };

            buttonContainer.appendChild(changeButton);
            buttonContainer.appendChild(deleteButton);

            listItem.appendChild(userInfoContainer);
            listItem.appendChild(buttonContainer);

            modalBody.appendChild(listItem);
        });
    }

    modalContent.appendChild(modalHeader);
    modalContent.appendChild(modalBody);
    modal.appendChild(modalContent);
    document.body.appendChild(modal);

    renderRoles();
}