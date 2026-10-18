// Página de nuevo pedido: catálogo de productos disponibles + resumen con el total.
// Al registrar, el backend valida el inventario; si falta un ingrediente, muestra el motivo y no guarda nada.
crearApp({
    data: () => ({
        cargando: true,
        error: '',
        productos: [],
        categorias: [],
        clientes: [],
        usuarios: [],
        busqueda: '',
        categoria: 0,
        idCliente: 0,
        idUsuario: 0,
        items: [],          // [{ producto, cantidad }]
        enviando: false,
        errorRegistro: '',
        registrado: null,   // pedido recién creado, para mostrar la confirmación
    }),
    computed: {
        productosFiltrados() {
            const texto = this.busqueda.trim().toLowerCase();
            return this.productos.filter(p =>
                (!this.categoria || p.idCategoria === this.categoria) &&
                (!texto || p.nombre.toLowerCase().includes(texto)));
        },
        total() {
            return this.items.reduce((suma, item) => suma + item.producto.precio * item.cantidad, 0);
        },
        unidades() {
            return this.items.reduce((suma, item) => suma + item.cantidad, 0);
        },
    },
    methods: {
        async cargar() {
            this.cargando = true;
            this.error = '';
            try {
                [this.productos, this.categorias, this.clientes, this.usuarios] = await Promise.all([
                    api.productos.listar({ disponible: true }),
                    api.categorias.listar(),
                    api.clientes.listar(),
                    api.usuarios.listar(),
                ]);
                // Mientras no exista el login, se propone como responsable al primer empleado
                const empleado = this.usuarios.find(u => u.rol === 'EMPLEADO') ?? this.usuarios[0];
                this.idUsuario = empleado?.idUsuario ?? 0;
            } catch (e) {
                this.error = e.message;
            } finally {
                this.cargando = false;
            }
        },
        cantidadEnPedido(idProducto) {
            return this.items.find(i => i.producto.idProducto === idProducto)?.cantidad ?? 0;
        },
        agregar(producto) {
            const item = this.items.find(i => i.producto.idProducto === producto.idProducto);
            if (item) item.cantidad++;
            else this.items.push({ producto, cantidad: 1 });
            this.errorRegistro = '';
        },
        cambiarCantidad(item, cambio) {
            item.cantidad += cambio;
            if (item.cantidad <= 0) this.items.splice(this.items.indexOf(item), 1);
            this.errorRegistro = '';
        },
        async registrar() {
            this.enviando = true;
            this.errorRegistro = '';
            try {
                this.registrado = await api.pedidos.crear({
                    idCliente: this.idCliente,
                    idUsuario: this.idUsuario,
                    items: this.items.map(i => ({ idProducto: i.producto.idProducto, cantidad: i.cantidad })),
                });
                this.items = [];
                this.idCliente = 0;
                notificar('exito', `Pedido #${this.registrado.idPedido} registrado.`);
                window.scrollTo({ top: 0, behavior: 'smooth' });
            } catch (e) {
                // Por ejemplo: "Inventario insuficiente de 'Gaseosa personal'..."; el pedido no se guardó
                this.errorRegistro = e.message;
            } finally {
                this.enviando = false;
            }
        },
    },
    mounted() {
        this.cargar();
    },
}).mount('#app');
