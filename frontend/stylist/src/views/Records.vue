<template>
  <div>
    <h2>服务记录</h2>
    <el-table :data="records" stripe>
      <el-table-column prop="customerName" label="顾客" />
      <el-table-column prop="date" label="日期" />
      <el-table-column prop="services" label="项目" />
      <el-table-column prop="totalPrice" label="收入" />
      <el-table-column prop="status" label="状态" />
    </el-table>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import api from "../api";

const records = ref([]);

onMounted(async () => {
  const res = await api.get("/stylist/service-records");
  records.value = res.data;
});
</script>
