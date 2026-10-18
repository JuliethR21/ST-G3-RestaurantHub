// Página de categorías: listar, crear, editar y eliminar
crearApp({
    data: () => ({
        cargando: true,
        error: '',
        categorias: [],
        productos: [],
        formulario: { abierto: false, id: null, guardando: false, error: '', datos: { nombre: '', descripcion: '' } },
    }),
    computed: {
        // Cuántos productos tiene cada categoría, para saber cuáles se pueden eliminar
        productosPorCategoria() {
            const conteo = {};
            for (const p of this.productos) conteo[p.idCategoria] = (conteo[p.idCategoria] || 0) + 1;
            return conteo;
        },
    },
    methods: {
        async cargar() {
            this.cargando = true;
            this.error = '';
            try {
                [this.categorias, this.productos] = await Promise.all([api.categorias.listar(), api.productos.listar()]);
            } catch (e) {
                this.error = e.message;
            } finally {
                this.cargando = false;
            }
        },
        abrirFormulario(categoria) {
            this.formulario = {
                abierto: true,
                id: categoria?.idCategoria ?? null,
                guardando: false,
                error: '',
                datos: { nombre: categoria?.nombre ?? '', descripcion: categoria?.descripcion ?? '' },
            };
        },
        async guardar() {
            this.formulario.guardando = true;
            this.formulario.error = '';
            try {
                const { id, datos } = this.formulario;
                if (id) await api.categorias.actualizar(id, datos);
                else await api.categorias.crear(datos);
                this.formulario.abierto = false;
                notificar('exito', id ? 'Categoría actualizada.' : 'Categoría creada.');
                this.cargar();
            } catch (e) {
                this.formulario.error = e.message;
            } finally {
                this.formulario.guardando = false;
            }
        },
        async eliminar(categoria) {
            if (!confirm(`¿Eliminar la categoría "${categoria.nombre}"?`)) return;
            try {
                await api.categorias.eliminar(categoria.idCategoria);
                notificar('exito', 'Categoría eliminada.');
                this.cargar();
            } catch (e) {
                notificar('error', e.message);
            }
        },
    },
    mounted() {
        this.cargar();
    },
}).mount('#app');
