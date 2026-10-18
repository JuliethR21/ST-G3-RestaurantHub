// Página de inicio: resumen con datos reales de pedidos, productos e inventario
crearApp({
    data: () => ({
        cargando: true,
        error: '',
        pedidos: [],
        productos: [],
        ingredientes: [],
        alertas: [],
        estados: ESTADOS_PEDIDO,
    }),
    computed: {
        conteoPorEstado() {
            const conteo = {};
            for (const p of this.pedidos) conteo[p.estado] = (conteo[p.estado] || 0) + 1;
            return conteo;
        },
        pedidosActivos() {
            return (this.conteoPorEstado.PENDIENTE || 0) + (this.conteoPorEstado.PREPARANDO || 0);
        },
        productosDisponibles() {
            return this.productos.filter(p => p.disponible).length;
        },
        ingredientesBajos() {
            return this.ingredientes.filter(i => i.bajoMinimo).length;
        },
        // La API ya devuelve los pedidos del más reciente al más antiguo
        pedidosRecientes() {
            return this.pedidos.slice(0, 5);
        },
    },
    methods: {
        async cargar() {
            this.cargando = true;
            this.error = '';
            try {
                [this.pedidos, this.productos, this.ingredientes, this.alertas] = await Promise.all([
                    api.pedidos.listar(),
                    api.productos.listar(),
                    api.ingredientes.listar(),
                    api.ingredientes.alertas(),
                ]);
            } catch (e) {
                this.error = e.message;
            } finally {
                this.cargando = false;
            }
        },
        verPedido(id) {
            window.location.href = `pedidos.html?id=${id}`;
        },
    },
    mounted() {
        this.cargar();
    },
}).mount('#app');
