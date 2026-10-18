// Cliente de la API: es el único archivo que conoce las rutas del backend.
// Las páginas llaman, por ejemplo, a api.pedidos.crear(datos) y reciben los datos ya convertidos de JSON.
// Si la API responde con error, se lanza un Error con el mensaje que envió el backend.
const api = (() => {
    async function pedir(metodo, ruta, cuerpo) {
        let respuesta;
        try {
            respuesta = await fetch(CONFIG.apiUrl + ruta, {
                method: metodo,
                headers: cuerpo ? { 'Content-Type': 'application/json' } : {},
                body: cuerpo ? JSON.stringify(cuerpo) : undefined,
            });
        } catch {
            throw new Error(`No se pudo conectar con la API (${CONFIG.apiUrl}). Verifica que el backend esté en ejecución.`);
        }

        if (respuesta.status === 204) return null;

        const datos = await respuesta.json().catch(() => null);
        if (!respuesta.ok) throw new Error(mensajeDeError(datos, respuesta.status));
        return datos;
    }

    // El backend responde { error: "..." } en los errores de negocio (404 y 409)
    // y { errors: { campo: ["..."] } } cuando falla la validación de los datos (400)
    function mensajeDeError(datos, status) {
        if (datos?.error) return datos.error;
        if (datos?.errors) return Object.values(datos.errors).flat().join(' ');
        return `La API respondió con un error (${status}).`;
    }

    // Convierte { estado: 'PENDIENTE', desde: '' } en "?estado=PENDIENTE", omitiendo los filtros vacíos
    function consulta(filtros = {}) {
        const parametros = new URLSearchParams();
        for (const [clave, valor] of Object.entries(filtros)) {
            if (valor !== '' && valor !== null && valor !== undefined) parametros.append(clave, valor);
        }
        const texto = parametros.toString();
        return texto ? `?${texto}` : '';
    }

    return {
        categorias: {
            listar: () => pedir('GET', '/categorias'),
            crear: datos => pedir('POST', '/categorias', datos),
            actualizar: (id, datos) => pedir('PUT', `/categorias/${id}`, datos),
            eliminar: id => pedir('DELETE', `/categorias/${id}`),
        },
        productos: {
            listar: filtros => pedir('GET', '/productos' + consulta(filtros)),
            obtener: id => pedir('GET', `/productos/${id}`),
            crear: datos => pedir('POST', '/productos', datos),
            actualizar: (id, datos) => pedir('PUT', `/productos/${id}`, datos),
            desactivar: id => pedir('DELETE', `/productos/${id}`),
        },
        pedidos: {
            listar: filtros => pedir('GET', '/pedidos' + consulta(filtros)),
            obtener: id => pedir('GET', `/pedidos/${id}`),
            crear: datos => pedir('POST', '/pedidos', datos),
            cambiarEstado: (id, estado) => pedir('PATCH', `/pedidos/${id}/estado`, { estado }),
        },
        ingredientes: {
            listar: () => pedir('GET', '/ingredientes'),
            alertas: () => pedir('GET', '/ingredientes/alertas'),
        },
        clientes: {
            listar: () => pedir('GET', '/clientes'),
        },
        usuarios: {
            listar: () => pedir('GET', '/usuarios'),
        },
    };
})();
