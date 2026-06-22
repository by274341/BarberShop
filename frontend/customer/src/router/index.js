import { createRouter, createWebHistory } from "vue-router";

const routes = [
  { path: "/", name: "Home", component: () => import("../views/Home.vue") },
  { path: "/login", name: "Login", component: () => import("../views/Login.vue") },
  { path: "/stylist/:id", name: "StylistDetail", component: () => import("../views/StylistDetail.vue") },
  { path: "/book", name: "Book", component: () => import("../views/Book.vue") },
  { path: "/orders", name: "Orders", component: () => import("../views/Orders.vue") },
  { path: "/profile", name: "Profile", component: () => import("../views/Profile.vue") }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
