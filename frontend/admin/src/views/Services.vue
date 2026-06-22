<template>
  <div>
    <h2>服务管理</h2>
    <el-button type="primary" @click="openAdd">添加服务</el-button>
    <el-table :data="services" stripe style="margin-top:10px">
      <el-table-column prop="name" label="名称" />
      <el-table-column prop="category" label="分类" />
      <el-table-column prop="price" label="价格" />
      <el-table-column prop="duration" label="时长(分钟)" />
      <el-table-column prop="isActive" label="状态">
        <template #default="{row}"><el-tag :type="row.isActive?'success':'danger'">{{ row.isActive?'上架':'下架' }}</el-tag></template>
      </el-table-column>
      <el-table-column label="操作">
        <template #default="{row}">
          <el-button size="small" @click="edit(row)">编辑</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog v-model="dialogVisible" :title="editing?'编辑服务':'添加服务'" width="400px">
      <el-form :model="form" label-width="100px">
        <el-form-item label="名称"><el-input v-model="form.name" /></el-form-item>
        <el-form-item label="分类"><el-input v-model="form.category" /></el-form-item>
        <el-form-item label="价格"><el-input-number v-model="form.price" :min="0" /></el-form-item>
        <el-form-item label="时长(分钟)"><el-input-number v-model="form.duration" :min="0" /></el-form-item>
        <el-form-item label="描述"><el-input v-model="form.description" type="textarea" /></el-form-item>
        <el-form-item label="图片URL"><el-input v-model="form.image" /></el-form-item>
        <el-form-item label="上架"><el-switch v-model="form.isActive" /></el-form-item>`n        <el-form-item label="参与促销"><el-switch v-model="form.isDiscountable" /></el-form-item>
      </el-form>
      <template #footer><el-button type="primary" @click="save">保存</el-button></template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from "vue";
import api from "../api";

const services = ref([]);
const dialogVisible = ref(false);
const editing = ref(false);
const form = reactive({ id:0, name:"", category:"", price:0, duration:30, description:"", image:"", isActive:true, isDiscountable:true });

onMounted(async () => { const res = await api.get("/services/all"); services.value = res.data; });

function openAdd() { editing.value=false; Object.assign(form, { id:0, name:"", category:"", price:0, duration:30, description:"", image:"", isActive:true, isDiscountable:true }); dialogVisible.value=true; }

function edit(row) { editing.value=true; Object.assign(form, row); dialogVisible.value=true; }

async function save() {
  if (editing.value) await api.put(`/services/${form.id}`, form);
  else await api.post("/services", form);
  dialogVisible.value = false;
  const res = await api.get("/services/all");
  services.value = res.data;
}
</script>
