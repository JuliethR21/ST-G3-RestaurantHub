// Página de productos: catálogo con filtros, crear, editar, desactivar y ver receta
const PRODUCTO_VACIO = { idCategoria: 0, nombre: '', descripcion: '', precio: 0, disponible: true };

let temporizadorBusqueda;

crearApp({
    data: () => ({
        cargando: true,
        error: '',
        productos: [],
        categorias: [],
        filtros: { nombre: '', idCategoria: '', disponible: '' },
        formulario: { abierto: false, id: null, guardando: false, error: '', datos: { ...PRODUCTO_VACIO } },
        receta: { abierto: false, producto: null, cargando: false, error: '', ingredientes: [] },
    }),
    methods: {
        async cargar() {
            this.cargando = true;
            this.error = '';
            try {
                this.productos = await api.productos.listar(this.filtros);
            } catch (e) {
                this.error = e.message;
            } finally {
                this.cargando = false;
            }
        },
        // Espera a que el usuario deje de escribir para no consultar la API en cada tecla
        buscarConPausa() {
            clearTimeout(temporizadorBusqueda);
            temporizadorBusqueda = setTimeout(() => this.cargar(), 350);
        },
        abrirFormulario(producto) {
            this.formulario = {
                abierto: true,
                id: producto?.idProducto ?? null,
                guardando: false,
                error: '',
                datos: producto
                    ? { idCategoria: producto.idCategoria, nombre: producto.nombre, descripcion: producto.descripcion ?? '', precio: producto.precio, disponible: producto.disponible }
                    : { ...PRODUCTO_VACIO },
            };
        },
        async guardar() {
            this.formulario.guardando = true;
            this.formulario.error = '';
            try {
                const { id, datos } = this.formulario;
                if (id) await api.productos.actualizar(id, datos);
                else await api.productos.crear(datos);
                this.formulario.abierto = false;
                notificar('exito', id ? 'Producto actualizado.' : 'Producto creado.');
                this.cargar();
            } catch (e) {
                this.formulario.error = e.message;
            } finally {
                this.formulario.guardando = false;
            }
        },
        async desactivar(producto) {
            if (!confirm(`¿Desactivar "${producto.nombre}"? Ya no se podrá agregar a pedidos nuevos.`)) return;
            try {
                await api.productos.desactivar(producto.idProducto);
                notificar('exito', `"${producto.nombre}" quedó como no disponible.`);
                this.cargar();
            } catch (e) {
                notificar('error', e.message);
            }
        },
        async verReceta(producto) {
            this.receta = { abierto: true, producto, cargando: true, error: '', ingredientes: [] };
            try {
                this.receta.ingredientes = (await api.productos.obtener(producto.idProducto)).receta;
            } catch (e) {
                this.receta.error = e.message;
            } finally {
                this.receta.cargando = false;
            }
        },
    },
    async mounted() {
        try {
            this.categorias = await api.categorias.listar();
        } catch {
            // Si falla, el error se mostrará al cargar los productos
        }
        this.cargar();
    },
}).mount('#app');
