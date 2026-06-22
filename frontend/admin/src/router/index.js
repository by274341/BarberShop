import { createRouter, createWebHistory } from "vue-router";

const routes = [
  { path: "/", redirect: "/dashboard" },
  { path: "/login", name: "Login", component: () => import("../views/Login.vue") },
  { path: "/dashboard", name: "Dashboard", component: () => import("../views/Dashboard.vue") },
  { path: "/appointments", name: "Appointments", component: () => import("../views/Appointments.vue") },
  { path: "/stylists", name: "Stylists", component: () => import("../views/Stylists.vue") },
  { path: "/services", name: "Services", component: () => import("../views/Services.vue") },
  { path: "/customers", name: "Customers", component: () => import("../views/Customers.vue") },
  { path: "/marketing", name: "Marketing", component: () => import("../views/Marketing.vue") },
  { path: "/statistics", name: "Statistics", component: () => import("../views/Statistics.vue") }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
