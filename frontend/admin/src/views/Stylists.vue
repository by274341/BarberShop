<template>
  <div>
    <h2>发型师管理</h2>
    <el-button type="primary" @click="openAdd">添加发型师</el-button>
    <el-table :data="stylists" stripe style="margin-top:10px">
      <el-table-column prop="nickname" label="姓名" />
      <el-table-column prop="level" label="等级" />
      <el-table-column prop="years" label="从业年限" />
      <el-table-column prop="specialty" label="擅长" />
      <el-table-column prop="status" label="状态">
        <template #default="{row}"><el-tag :type="row.status==='Active'?'success':'danger'">{{ row.status==='Active'?'在岗':'停用' }}</el-tag></template>
      </el-table-column>
      <el-table-column prop="avgRating" label="评分" />
      <el-table-column label="操作">
        <template #default="{row}">
          <el-button size="small" @click="toggle(row.id)">{{ row.status==='Active'?'停用':'启用' }}</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog v-model="addVisible" title="添加发型师" width="400px">
      <el-form :model="form" label-width="80px">
        <el-form-item label="姓名"><el-input v-model="form.nickname" /></el-form-item>
        <el-form-item label="等级"><el-select v-model="form.level"><el-option v-for="l in ['初级','中级','高级','总监']" :key="l" :label="l" :value="l" /></el-select></el-form-item>
        <el-form-item label="从业年限"><el-input-number v-model="form.years" :min="0" /></el-form-item>
        <el-form-item label="擅长"><el-input v-model="form.specialty" placeholder="逗号分隔" /></el-form-item>
        <el-form-item label="介绍"><el-input v-model="form.description" type="textarea" /></el-form-item>
      </el-form>
      <template #footer><el-button type="primary" @click="add">确定</el-button></template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from "vue";
import api from "../api";

const stylists = ref([]);
const addVisible = ref(false);
const form = reactive({ nickname:"", level:"初级", years:0, specialty:"", description:"" });

onMounted(async () => { const res = await api.get("/stylists/all"); stylists.value = res.data; });

function openAdd() { form.nickname=""; form.level="初级"; form.years=0; form.specialty=""; form.description=""; addVisible.value=true; }

async function add() {
  await api.post("/stylists", form);
  addVisible.value = false;
  const res = await api.get("/stylists/all");
  stylists.value = res.data;
}

async function toggle(id) {
  await api.put(`/stylists/${id}/status`);
  const res = await api.get("/stylists/all");
  stylists.value = res.data;
}
</script>
