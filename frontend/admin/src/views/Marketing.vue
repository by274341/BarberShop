<template>
  <div>
    <h2>优惠券管理</h2>
    <el-button type="primary" @click="couponVisible=true">创建优惠券</el-button>
    <el-table :data="coupons" stripe style="margin-top:10px">
      <el-table-column prop="name" label="名称" />
      <el-table-column prop="faceValue" label="面额" />
      <el-table-column prop="minAmount" label="使用门槛" />
      <el-table-column prop="validFrom" label="开始" :formatter="(r)=>r.validFrom?.slice(0,10)" />
      <el-table-column prop="validTo" label="结束" :formatter="(r)=>r.validTo?.slice(0,10)" />
      <el-table-column prop="issuedCount" label="已发放" />
      <el-table-column prop="totalCount" label="总量" />
    </el-table>

    <el-dialog v-model="couponVisible" title="创建优惠券" width="400px">
      <el-form :model="cForm" label-width="100px">
        <el-form-item label="名称"><el-input v-model="cForm.name" /></el-form-item>
        <el-form-item label="面额"><el-input-number v-model="cForm.faceValue" :min="0" /></el-form-item>
        <el-form-item label="使用门槛"><el-input-number v-model="cForm.minAmount" :min="0" /></el-form-item>
        <el-form-item label="开始日期"><el-date-picker v-model="cForm.validFrom" type="date" /></el-form-item>
        <el-form-item label="结束日期"><el-date-picker v-model="cForm.validTo" type="date" /></el-form-item>
        <el-form-item label="发放数量"><el-input-number v-model="cForm.totalCount" :min="1" /></el-form-item>
      </el-form>
      <template #footer><el-button type="primary" @click="createCoupon">确定</el-button></template>
    </el-dialog>

    <h2 style="margin-top:30px">限时活动</h2>
    <el-button type="primary" @click="promoVisible=true">创建活动</el-button>
    <el-table :data="promotions" stripe style="margin-top:10px">
      <el-table-column prop="name" label="名称" />
      <el-table-column prop="serviceName" label="服务" />
      <el-table-column prop="discountRate" label="折扣" :formatter="(r)=>Math.round(r.discountRate*10)+'折'" />
      <el-table-column prop="startTime" label="开始" :formatter="(r)=>r.startTime?.slice(0,10)" />
      <el-table-column prop="endTime" label="结束" :formatter="(r)=>r.endTime?.slice(0,10)" />
      <el-table-column prop="isActive" label="状态">
        <template #default="{row}"><el-tag :type="row.isActive?'success':'danger'">{{ row.isActive?'生效中':'已停用' }}</el-tag></template>
      </el-table-column>
      <el-table-column label="操作">
        <template #default="{row}">
          <el-button size="small" @click="togglePromo(row.id)">{{ row.isActive?'停用':'启用' }}</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog v-model="promoVisible" title="创建限时活动" width="400px">
      <el-form :model="pForm" label-width="100px">
        <el-form-item label="名称"><el-input v-model="pForm.name" /></el-form-item>
        <el-form-item label="服务"><el-select v-model="pForm.serviceId"><el-option v-for="s in services" :key="s.id" :label="s.name" :value="s.id" /></el-select></el-form-item>
        <el-form-item label="折扣"><el-input-number v-model="pForm.discountRate" :min="0.1" :max="1" :step="0.1" /></el-form-item>
        <el-form-item label="开始"><el-date-picker v-model="pForm.startTime" type="datetime" /></el-form-item>
        <el-form-item label="结束"><el-date-picker v-model="pForm.endTime" type="datetime" /></el-form-item>
      </el-form>
      <template #footer><el-button type="primary" @click="createPromo">确定</el-button></template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from "vue";
import api from "../api";

const coupons = ref([]);
const promotions = ref([]);
const services = ref([]);
const couponVisible = ref(false);
const promoVisible = ref(false);
const cForm = reactive({ name:"", faceValue:0, minAmount:0, validFrom:"", validTo:"", totalCount:100 });
const pForm = reactive({ name:"", serviceId:0, discountRate:0.8, startTime:"", endTime:"" });

onMounted(async () => {
  const [c, p, s] = await Promise.all([
    api.get("/coupons"), api.get("/promotions"), api.get("/services/all")
  ]);
  coupons.value = c.data; promotions.value = p.data; services.value = s.data;
});

async function createCoupon() { await api.post("/coupons", cForm); couponVisible.value=false; loadCoupons(); }
async function createPromo() { await api.post("/promotions", pForm); promoVisible.value=false; loadPromos(); }
async function togglePromo(id) { await api.put(`/promotions/${id}/toggle`); loadPromos(); }

async function loadCoupons() { const r = await api.get("/coupons"); coupons.value = r.data; }
async function loadPromos() { const r = await api.get("/promotions"); promotions.value = r.data; }
</script>
