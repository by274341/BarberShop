<template>
  <div>
    <h2>预约日历</h2>
    <el-date-picker v-model="selectedDate" type="date" @change="load" />
    <el-table :data="appointments" stripe style="margin-top:15px">
      <el-table-column prop="customerName" label="顾客" />
      <el-table-column prop="services" label="项目" />
      <el-table-column prop="timeSlot" label="时段" />
      <el-table-column prop="totalPrice" label="金额" />
      <el-table-column prop="status" label="状态" />
    </el-table>
    <el-empty v-if="appointments.length===0" description="该天暂无预约" />
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import api from "../api";

const selectedDate = ref(new Date());
const appointments = ref([]);

onMounted(() => load());

async function load() {
  const date = selectedDate.value.toISOString().slice(0, 10);
  const res = await api.get(`/appointments/my?date=${date}`);
  appointments.value = res.data.filter(a => a.appointmentDate?.startsWith(date));
}
</script>
