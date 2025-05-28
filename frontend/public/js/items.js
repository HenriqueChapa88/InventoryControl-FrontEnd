async function loadItems() {
    try {
        const response = await fetch('/api/items', {
            headers: getAuthHeader()
        });
        
        if (!response.ok) throw new Error('Failed to load items');
        
        const items = await response.json();
        renderItems(items);
    } catch (error) {
        console.error('Error loading items:', error);
    }
}

function renderItems(items) {
    const tbody = document.querySelector('#items-table tbody');
    tbody.innerHTML = items.map(item => `
        <tr>
            <td>${item.id}</td>
            <td>${item.name}</td>
            <td>${item.sku}</td>
            <td>${item.quantity}</td>
            <td>$${item.unitPrice.toFixed(2)}</td>
            <td>${item.supplierName}</td>
            <td>
                <button class="action-btn edit-btn" data-id="${item.id}">Edit</button>
                <button class="action-btn delete-btn" data-id="${item.id}">Delete</button>
                <button class="action-btn stock-btn" data-id="${item.id}">Stock</button>
            </td>
        </tr>
    `).join('');

    document.querySelectorAll('.edit-btn').forEach(btn => {
        btn.addEventListener('click', () => editItem(btn.dataset.id));
    });
    
    document.querySelectorAll('.delete-btn').forEach(btn => {
        btn.addEventListener('click', () => deleteItem(btn.dataset.id));
    });
}

async function editItem(id) {
    try {
        const response = await fetch(`/api/items/${id}`, {
            headers: getAuthHeader()
        });
        
        if (!response.ok) throw new Error('Failed to load item');
        
        const item = await response.json();
        showItemForm(item);
    } catch (error) {
        alert(error.message);
    }
}

async function deleteItem(id) {
    if (!confirm('Are you sure you want to delete this item?')) return;
    
    try {
        const response = await fetch(`/api/items/${id}`, {
            method: 'DELETE',
            headers: getAuthHeader()
        });
        
        if (!response.ok) throw new Error('Failed to delete item');
        
        loadItems();
    } catch (error) {
        alert(error.message);
    }
}

function showItemForm(item = null) {

}

if (currentToken) {
    loadItems();
}