<template>
  <div>
    <h2>今日预约</h2>
    <el-table :data="appointments" stripe>
      <el-table-column prop="customerName" label="顾客" />
      <el-table-column prop="services" label="项目" />
      <el-table-column prop="timeSlot" label="时段" />
      <el-table-column prop="totalPrice" label="金额" />
      <el-table-column prop="status" label="状态">
        <template #default="{row}">
          <el-tag :type="statusType(row.status)">{{ statusText(row.status) }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="200">
        <template #default="{row}">
          <template v-if="row.status==='Pending'">
            <el-button size="small" type="success" @click="confirm(row.id)">确认接单</el-button>
            <el-button size="small" type="danger" @click="reject(row.id)">无法服务</el-button>
          </template>
        </template>
      </el-table-column>
    </el-table>
    <el-empty v-if="appointments.length===0" description="今天暂无预约" />
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import api from "../api";

const appointments = ref([]);

onMounted(async () => {
  const res = await api.get("/appointments/today");
  appointments.value = res.data;
});

async function confirm(id) {
  await api.put(`/appointments/${id}/confirm`);
  const a = appointments.value.find(x => x.id === id);
  if (a) a.status = "Confirmed";
}

async function reject(id) {
  await api.put(`/appointments/${id}/reject`);
  const a = appointments.value.find(x => x.id === id);
  if (a) a.status = "Rejected";
}

function statusType(s) {
  return { Pending: "warning", Confirmed: "success", Completed: "info", Cancelled: "danger", Rejected: "danger" }[s] || "info";
}
function statusText(s) {
  return { Pending: "待确认", Confirmed: "已确认", Completed: "已完成", Cancelled: "已取消", Rejected: "已拒绝" }[s] || s;
}
</script>
