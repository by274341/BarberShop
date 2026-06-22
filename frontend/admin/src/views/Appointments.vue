<template>
  <div>
    <h2>预约管理</h2>
    <el-form inline>
      <el-form-item><el-date-picker v-model="filterDate" type="date" placeholder="日期" @change="load" /></el-form-item>
      <el-form-item>
        <el-select v-model="filterStatus" placeholder="状态" clearable @change="load">
          <el-option label="待确认" value="Pending" /><el-option label="已确认" value="Confirmed" />
          <el-option label="已完成" value="Completed" /><el-option label="已取消" value="Cancelled" />
        </el-select>
      </el-form-item>
    </el-form>
    <el-table :data="appointments" stripe>
      <el-table-column prop="customerName" label="顾客" />
      <el-table-column prop="stylistName" label="发型师" />
      <el-table-column prop="services" label="项目" />
      <el-table-column prop="appointmentDate" label="日期" :formatter="(r)=>r.appointmentDate?.slice(0,10)" />
      <el-table-column prop="timeSlot" label="时段" />
      <el-table-column prop="totalPrice" label="金额" />
      <el-table-column prop="status" label="状态">
        <template #default="{row}"><el-tag :type="st(row.status)">{{ stt(row.status) }}</el-tag></template>
      </el-table-column>
      <el-table-column label="操作" width="200">
        <template #default="{row}">
          <el-button v-if="row.status==='Pending'" size="small" type="success" @click="act(row.id,'confirm')">确认</el-button>
          <el-button v-if="row.status!=='Completed'&&row.status!=='Cancelled'" size="small" type="primary" @click="act(row.id,'complete')">完成</el-button>
          <el-button v-if="row.status!=='Cancelled'" size="small" type="danger" @click="act(row.id,'cancel')">取消</el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import api from "../api";

const appointments = ref([]);
const filterDate = ref("");
const filterStatus = ref("");

onMounted(() => load());

async function load() {
  const params = {};
  if (filterDate.value) params.date = filterDate.value.toISOString().slice(0,10);
  if (filterStatus.value) params.status = filterStatus.value;
  const res = await api.get("/appointments/admin-all", { params });
  appointments.value = res.data;
}

async function act(id, action) { await api.put(`/appointments/${id}/${action}`); load(); }

function st(s) { return { Pending:"warning",Confirmed:"success",Completed:"info",Cancelled:"danger" }[s]||"info"; }
function stt(s) { return { Pending:"待确认",Confirmed:"已确认",Completed:"已完成",Cancelled:"已取消",Rejected:"已拒绝" }[s]||s; }
</script>
