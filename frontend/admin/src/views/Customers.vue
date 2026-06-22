<template>
  <div>
    <h2>顾客管理</h2>
    <el-table :data="customers" stripe>
      <el-table-column prop="nickname" label="昵称" />
      <el-table-column prop="phone" label="手机号" />
      <el-table-column prop="totalOrders" label="消费次数" />
      <el-table-column prop="totalSpent" label="累计消费" />
      <el-table-column prop="createdAt" label="注册时间" :formatter="(r)=>r.createdAt?.slice(0,10)" />
    </el-table>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import api from "../api";

const customers = ref([]);

onMounted(async () => {
  const res = await api.get("/customers");
  customers.value = res.data;
});
</script>
