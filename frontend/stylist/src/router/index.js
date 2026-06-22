import { createRouter, createWebHistory } from "vue-router";

const routes = [
  { path: "/", redirect: "/today" },
  { path: "/login", name: "Login", component: () => import("../views/Login.vue") },
  { path: "/today", name: "Today", component: () => import("../views/Today.vue") },
  { path: "/calendar", name: "Calendar", component: () => import("../views/Calendar.vue") },
  { path: "/schedule", name: "Schedule", component: () => import("../views/Schedule.vue") },
  { path: "/records", name: "Records", component: () => import("../views/Records.vue") },
  { path: "/stats", name: "Stats", component: () => import("../views/Stats.vue") }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
