async function loadSuppliers() {
    try {
        const response = await fetch('/api/suppliers', {
            headers: getAuthHeader()
        });
        
        if (!response.ok) throw new Error('Failed to load suppliers');
        
        const suppliers = await response.json();
        renderSuppliers(suppliers);
    } catch (error) {
        console.error('Error loading suppliers:', error);
    }
}

function renderSuppliers(suppliers) {
    const tbody = document.querySelector('#suppliers-table tbody');
    tbody.innerHTML = suppliers.map(supplier => `
        <tr>
            <td>${supplier.id}</td>
            <td>${supplier.name}</td>
            <td>${supplier.cnpj}</td>
            <td>${supplier.phone || '-'}</td>
            <td>
                <button class="action-btn edit-btn" data-id="${supplier.id}">Edit</button>
                <button class="action-btn delete-btn" data-id="${supplier.id}">Delete</button>
            </td>
        </tr>
    `).join('');

    document.querySelectorAll('.edit-btn').forEach(btn => {
        btn.addEventListener('click', () => editSupplier(btn.dataset.id));
    });
    
    document.querySelectorAll('.delete-btn').forEach(btn => {
        btn.addEventListener('click', () => deleteSupplier(btn.dataset.id));
    });
}

async function editSupplier(id) {
    try {
        const response = await fetch(`/api/suppliers/${id}`, {
            headers: getAuthHeader()
        });
        
        if (!response.ok) throw new Error('Failed to load supplier');
        
        const supplier = await response.json();
        showSupplierForm(supplier);
    } catch (error) {
        alert(error.message);
    }
}

async function deleteSupplier(id) {
    if (!confirm('Are you sure you want to delete this supplier?')) return;
    
    try {
        const response = await fetch(`/api/suppliers/${id}`, {
            method: 'DELETE',
            headers: getAuthHeader()
        });
        
        if (!response.ok) throw new Error('Failed to delete supplier');
        
        loadSuppliers();
    } catch (error) {
        alert(error.message);
    }
}

function showSupplierForm(supplier = null) {
   
}

if (currentToken) {
    loadSuppliers();
}