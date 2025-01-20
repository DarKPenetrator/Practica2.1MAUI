# README - Práctica 2.1

## 1. Aplicación MAUI con Consumo de API REST
1.1. **Listado de Usuarios:**  
Se muestra un listado de usuarios obtenido de MockAPI.

1.2. **Añadir Usuario:**  
La aplicación permite añadir nuevos usuarios a través de un formulario interactivo.

1.3. **Actualizar Usuario:**  
Se puede actualizar la información de un usuario existente. Se mantienen actualizados los datos como nombre, correo y avatar.

1.4. **Eliminar Usuario:**  
Los usuarios pueden ser eliminados desde la lista interactiva.

<br>

## 2. Estructura y Diseño

3.1. **MVVM Implementado:**  
Se utiliza la estructura correcta de MVVM con ViewModels y Models separados.

3.2. **Diseño Interactivo:**  
La interfaz incluye botones para todas las operaciones CRUD, con estilos personalizados y colores consistentes.

3.3. **Validaciones:**  
Se implementan validaciones para manejar errores HTTP y mostrar mensajes claros al usuario.

3.4. **FlexLayout y Adaptabilidad:**  
Se utiliza un diseño adaptable con `FlexLayout` para que la interfaz sea clara y funcional en cualquier tamaño de pantalla.

<br>

## 3. Consumo de API Pública Real
- **API de Gravatar:**  
Se utiliza el endpoint público de Gravatar para obtener avatares asociados a los correos electrónicos.  
Se consume solo mediante peticiones **GET**.