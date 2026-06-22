<template>
  <div class="login-page">
    <el-card class="login-card">
      <h2>顾客登录</h2>
      <el-form :model="form" label-width="80px">
        <el-form-item label="用户名"><el-input v-model="form.username" /></el-form-item>
        <el-form-item label="密码"><el-input v-model="form.password" type="password" /></el-form-item>
        <el-form-item>
          <el-button type="primary" @click="login" :loading="loading">登录</el-button>
          <el-button @click="showRegister = true">注册</el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-dialog v-model="showRegister" title="顾客注册" width="400px">
      <el-form :model="regForm" label-width="80px">
        <el-form-item label="用户名"><el-input v-model="regForm.username" /></el-form-item>
        <el-form-item label="密码"><el-input v-model="regForm.password" type="password" /></el-form-item>
        <el-form-item label="昵称"><el-input v-model="regForm.nickname" /></el-form-item>
        <el-form-item label="手机号"><el-input v-model="regForm.phone" /></el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="register" type="primary">注册</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, reactive } from "vue";
import { useRouter } from "vue-router";
import api from "../api";

const router = useRouter();
const loading = ref(false);
const showRegister = ref(false);
const form = reactive({ username: "", password: "" });
const regForm = reactive({ username: "", password: "", nickname: "", phone: "" });

async function login() {
  loading.value = true;
  try {
    const res = await api.post("/auth/login", form);
    localStorage.setItem("token", res.data.token);
    localStorage.setItem("user", JSON.stringify(res.data));
    router.push("/");
  } catch (e) {
    alert(e.response?.data?.message || "登录失败");
  } finally {
    loading.value = false;
  }
}

async function register() {
  try {
    const res = await api.post("/auth/register", regForm);
    localStorage.setItem("token", res.data.token);
    localStorage.setItem("user", JSON.stringify(res.data));
    showRegister.value = false;
    router.push("/");
  } catch (e) {
    alert(e.response?.data?.message || "注册失败");
  }
}
</script>

<style scoped>
.login-page { display: flex; justify-content: center; align-items: center; min-height: 80vh; }
.login-card { width: 400px; }
h2 { text-align: center; margin-bottom: 20px; }
</style>
