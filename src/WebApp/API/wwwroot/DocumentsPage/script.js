import {tokenStorage, showForm, createLoginForm, createRegisterForm} from "../Authorization/script.js";


document.addEventListener('DOMContentLoaded', async () => {
    const projects = await getProjects();
    await loadProjectTable(projects);
});


function loadProjectTable(projectArray) {
    
    const container = document.getElementById('projectTableContainer');
    container.id = 'projectTableContainer';
    document.body.appendChild(container);

    const table = document.createElement('table');
    table.setAttribute('border', '1');

    const headerRow = document.createElement('tr');
    const headers = ['Name', 'Created At', 'Actions'];

    headers.forEach(headerText => {
        const th = document.createElement('th');
        th.textContent = headerText;
        headerRow.appendChild(th);
    });
    table.appendChild(headerRow);
        
        projectArray.forEach(project => {
        const row = document.createElement('tr');

        const nameCell = document.createElement('td');
        nameCell.textContent = project.name;
        row.appendChild(nameCell);

        const dateCell = document.createElement('td');
        const date = new Date(project.createdAt).toLocaleString();
        dateCell.textContent = date;
        row.appendChild(dateCell);

        const actionsCell = document.createElement('td');

        const downloadButton = document.createElement('button');
        downloadButton.textContent = 'Download';
        downloadButton.addEventListener('click', () => downloadDocument(project.documentId));
        actionsCell.appendChild(downloadButton);

        const openButton = document.createElement('button');
        openButton.textContent = 'Open';
        openButton.addEventListener('click', () => openDocument(project.documentId));
        actionsCell.appendChild(openButton);

        const renameButton = document.createElement('button');
        renameButton.textContent = 'Rename';
        renameButton.addEventListener('click', () => renameDocument(project.documentId));
        actionsCell.appendChild(renameButton);

        const deleteButton = document.createElement('button');
        deleteButton.textContent = 'Delete';
        deleteButton.addEventListener('click', () => deleteProject(project.documentId));
        actionsCell.appendChild(deleteButton);

        row.appendChild(actionsCell);

        table.appendChild(row);
    });

    container.innerHTML = '';
    container.appendChild(table);
}


async function getProjects() {
    let token = tokenStorage.get();

    try {
        const response = await fetch(`/api/documents/get`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
        });

        if (response.ok) {
            const responseData = await response.json();
            return responseData.projects;
        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm, '/auth/signin', 'Sign In');

            if (loginSuccessful) {
                return await getProjects(); 
            }
        } else {
            const data = await response.json();
            alert(data);
            return data; 
        }
    } catch (error) {
        alert(error);
        throw error; 
    }
}

async function renameProject(newName, documentId) {
    
    // newName из поля с окном изменения
    let token = tokenStorage.get();
    
    try {
        const response = await fetch(`/api/documents/rename`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({
                Name: newName,
                DocumentId: documentId
            })
        });

        if (response.ok) {
            const newName = await response.json();
            console.log("Изменено на" + newName);
        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm, '/auth/signin', 'Sign In');

            if (loginSuccessful) {
            }
        } else {
            const data = await response.json();
            alert(data);
        }
    } catch (error) {
        alert(error);
    }
}

async function deleteProject(documentId) {
    let token = tokenStorage.get();

    try {
        const response = await fetch(`/api/documents/delete`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify( documentId )
        });

        if (response.ok) {
            const projects = await getProjects();
            await loadProjectTable(projects);
        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm, '/auth/signin', 'Sign In');

            if (loginSuccessful) {
            }
        } else {
            const data = await response.json();
            alert(data);
        }
    } catch (error) {
        alert(error);
    }
} 