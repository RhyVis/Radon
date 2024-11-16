<script setup lang="tsx">
import { RefreshIcon } from "tdesign-icons-vue-next";
import { assembleSrc, charaList } from "@/pages/draw/pjsk-sticker/scripts/define";
import { useToggle } from "@vueuse/core";

const emit = defineEmits(["select"]);

const [visible, setVisible] = useToggle(false);
const renderIcon = () => <RefreshIcon />;

const handleDrawer = () => setVisible();
const handleSelect = (index: number) => {
  emit("select", index);
  handleDrawer();
};
</script>

<template>
  <t-button @click="handleDrawer">选择角色</t-button>
  <t-drawer v-model:visible="visible" size="360px" :footer="false" header="Pick one!">
    <t-row :gutter="[2, 0]">
      <t-col v-for="(chara, index) in charaList" :span="4" :key="index">
        <span class="r-chara-select-option" @click="handleSelect(index)">
          <t-image
            class="r-chara-select-inner-img"
            fit="contain"
            :src="assembleSrc(chara.img)"
            :loading="renderIcon"
            :lazy="true"
            style="height: 115px; width: 115px"
          />
        </span>
      </t-col>
    </t-row>
  </t-drawer>
</template>

<style scoped>
.r-chara-select-option {
  text-align: center;

  .r-chara-select-inner-img {
    max-width: 124px;
    max-height: 124px;
  }
}
</style>
